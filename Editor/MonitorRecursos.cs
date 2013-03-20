using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Fang3D.Editor
{
    public class MonitorRecursos
    {
        static private Queue<FileSystemEventArgs> colaPendiente = new Queue<FileSystemEventArgs>();
        static private FileSystemWatcher fileSystemWatcher;

        static public void StartMonitorearRecursos()
        {
            if (Directory.Exists("Resources"))
            {
                System.String[] archs = Directory.GetFiles("Resources");

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

                fileSystemWatcher = new FileSystemWatcher(Path.GetFullPath("Resources"));
                fileSystemWatcher.Changed += new FileSystemEventHandler(fileSystemWatcher_Changed);
                fileSystemWatcher.Created += new FileSystemEventHandler(fileSystemWatcher_Created);
                fileSystemWatcher.Deleted += new FileSystemEventHandler(fileSystemWatcher_Deleted);
                fileSystemWatcher.Renamed += new RenamedEventHandler(fileSystemWatcher_Renamed);

                fileSystemWatcher.EnableRaisingEvents = true;
            }
        }

        static public void MonitorearRecursos()
        {
            if (fileSystemWatcher == null)
                return;

            lock (colaPendiente)
            {
                if (colaPendiente.Count > 0)
                {
                    Scene.PushCurrentScene();

                    try
                    {
                        FormPrincipal.Instance.sceneEdit.MakeCurrent();

                        while (colaPendiente.Count > 0)
                        {
                            FileSystemEventArgs ev = colaPendiente.Dequeue();

                            if (ev.ChangeType == WatcherChangeTypes.Changed)
                            {
                                ProcessChanged(ev);
                            }
                            else if (ev.ChangeType == WatcherChangeTypes.Created)
                            {
                                //Intento procesarlo primero como Changed, asi si es un archivo que se habia borrado pero se creo de nuevo, se
                                //recupera la relación perdida.
                                if (!ProcessChanged(ev))
                                    ProcessCreated(ev);
                            }
                            else if (ev.ChangeType == WatcherChangeTypes.Deleted)
                            {
                                ProcessDeleted(ev);
                            }
                            else if (ev.ChangeType == WatcherChangeTypes.Renamed)
                            {
                                //Intento procesarlo primero como Changed, asi si es un archivo que se habia borrado pero se creo de nuevo, se
                                //recupera la relación perdida.
                                if (!ProcessChanged(ev))
                                    if (!ProcessRenamed(ev))
                                        ProcessCreated(ev);
                            }
                        }
                    }
                    finally
                    {
                        Scene.PopCurrentScene();
                    }
                }
            }
        }

        static private bool ProcessRenamed(FileSystemEventArgs ev)
        {
            RenamedEventArgs ev2 = (RenamedEventArgs)ev;

            foreach (Resource res in Resource.AllResources)
            {
                if (res.FileName != null && Path.GetFileName(res.FileName).Equals(ev2.OldName, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    res.FileName = "Resources\\" + ev2.Name;

                    return true;
                }
            }

            return false;
        }

        static private bool ProcessDeleted(FileSystemEventArgs ev)
        {
            foreach (Resource res in Resource.AllResources)
            {
                if (res.FileName != null && Path.GetFileName(res.FileName).Equals(ev.Name, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    res.OnFileDeleted();

                    FormPrincipal.Instance.menuComponentesValido = false;

                    if (FormPrincipal.Instance.vistaConArbolEdit.editorEntidad.Element == res)
                        FormPrincipal.Instance.vistaConArbolEdit.editorEntidad.Update();

                    if (res is Texture2D)
                    {
                        FormPrincipal.Instance.vistaConArbolEdit.vistaEscena.render.UnloadTexture((Texture2D)res);
                        FormPrincipal.Instance.vistaConArbolPlay.vistaEscena.render.UnloadTexture((Texture2D)res);
                    }
                    return true;
                }
            }

            return false;
        }

        static private bool ProcessCreated(FileSystemEventArgs ev)
        {
            if (Path.GetExtension(ev.Name).Equals(".cs", System.StringComparison.InvariantCultureIgnoreCase))
            {
                ScriptResource script = new ScriptResource();
                script.FileName = "Resources\\" + ev.Name;
            }
            else if (Path.GetExtension(ev.Name).Equals(".png", System.StringComparison.InvariantCultureIgnoreCase))
            {
                Texture2D texture = new Texture2D();
                texture.FileName = "Resources\\" + ev.Name;
            }
            else if (Path.GetExtension(ev.Name).Equals(".dae", System.StringComparison.InvariantCultureIgnoreCase))
            {
                Mesh mesh = new Mesh();
                mesh.FileName = "Resources\\" + ev.Name;
            }
            else
                return false;

            FormPrincipal.Instance.menuComponentesValido = false;
            return true;
        }

        static private bool ProcessChanged(FileSystemEventArgs ev)
        {
            foreach (Resource res in Resource.AllResources)
            {
                if (res.FileName != null && Path.GetFileName(res.FileName).Equals(ev.Name, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    System.Threading.Thread.Sleep(100);
                    res.OnFileChanged();

                    FormPrincipal.Instance.menuComponentesValido = false;

                    if (FormPrincipal.Instance.vistaConArbolEdit.editorEntidad.Element == res)
                        FormPrincipal.Instance.vistaConArbolEdit.editorEntidad.ActualizarValores();

                    if (res is Texture2D)
                    {
                        FormPrincipal.Instance.vistaConArbolEdit.vistaEscena.render.UnloadTexture((Texture2D)res);
                        FormPrincipal.Instance.vistaConArbolPlay.vistaEscena.render.UnloadTexture((Texture2D)res);
                    }

                    return true;
                }
            }

            return false;
        }

        static void fileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            lock (colaPendiente)
                colaPendiente.Enqueue(e);
        }

        static void fileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            lock (colaPendiente)
                colaPendiente.Enqueue(e);
        }

        static void fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            lock (colaPendiente)
                colaPendiente.Enqueue(e);
        }

        static void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            lock (colaPendiente)
                colaPendiente.Enqueue(e);
        }
    }
}
