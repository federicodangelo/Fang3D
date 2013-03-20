using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;
using OpenTK.Input;
using OpenTK.Platform;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.Reflection;
using ICSharpCode.SharpZipLib.Zip;

namespace Fang3D.Player
{
    static class Program
    {
        static List<String> filesToDelete;
        static String directoryToDelete;
        static String sceneFileName;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main(String[] args)
        {
            LogWriter.Init();

            if (args.Length == 1)
            {
                sceneFileName = args[0];
                LoadResources(Path.Combine(RunningPath, "Resources"));
            }
            else
            {
                String zipFileName = Path.Combine(RunningPath, "Resources.zip");

                if (File.Exists(zipFileName))
                {
                    String tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                    Directory.CreateDirectory(tempPath);

                    directoryToDelete = tempPath;
                    filesToDelete = new List<string>();

                    UnzipFiles(zipFileName, tempPath);

                    LoadResources(tempPath);
                }
            }

            if (sceneFileName != null && File.Exists(sceneFileName))
            {
                using (VistaEscena vista = new VistaEscena())
                {
                    vista.scene = new Scene(false);
                    vista.scene.MakeCurrent();

                    Scenes.Serializer.SceneSerializer.Load(vista.scene, sceneFileName);

                    //Borro los archivos temporales una vez cargados todos los recursos
                    if (filesToDelete != null)
                        foreach (String fn in filesToDelete)
                            File.Delete(fn);

                    if (directoryToDelete != null)
                        Directory.Delete(directoryToDelete);

                    vista.WindowBorder = WindowBorder.Fixed;

                    vista.Run(30.0, 60.0);
                }
            }
        }

        static public String RunningPath
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

        static private void LoadResources(String from)
        {
            StaticResources.Init();

            if (Directory.Exists(from))
            {
                System.String[] archs = Directory.GetFiles(from);

                foreach (System.String arch in archs)
                {
                    if (Path.GetExtension(arch).Equals(".cs", System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        ScriptResource script = new ScriptResource();
                        script.FileName = arch;
                    }
                    else if (Path.GetExtension(arch).Equals(".png", System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        Texture2D texture = new Texture2D();
                        texture.FileName = arch;
                    }
                    else if (Path.GetExtension(arch).Equals(".dae", System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        Mesh mesh = new Mesh();
                        mesh.FileName = arch;
                    }
                }
            }
        }

        static private void UnzipFiles(String zipFileName, String tempPath)
        {
            Console.WriteLog("Unzipping resources file: " + zipFileName + " in directory " + tempPath);

            FileStream fs = File.OpenRead(zipFileName);

            try
            {
                using (ZipInputStream s = new ZipInputStream(fs))
                {
                    ZipEntry theEntry;

                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);

                        // create directory
                        //if (directoryName.Length > 0)
                        //{
                        //    Directory.CreateDirectory(directoryName);
                        //}

                        if (fileName != String.Empty)
                        {
                            String fullName = Path.Combine(tempPath, fileName);

                            using (FileStream streamWriter = File.Create(fullName))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }

                                filesToDelete.Add(fullName);
                            }

                            Console.WriteLog("Unzipped file: " + fileName);

                            if (fileName.EndsWith(".f3d", StringComparison.InvariantCultureIgnoreCase))
                                sceneFileName = Path.Combine(tempPath, fileName);
                        }
                    }
                }

                LoadResources(tempPath);
            }
            finally
            {
                fs.Close();
            }
        }
    }
}
