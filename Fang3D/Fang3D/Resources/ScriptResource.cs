using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.CodeDom.Compiler;

namespace Fang3D
{
    public class ScriptResource : Resource
    {
        private Type scriptType;

        public ScriptResource()
        {
            Name = "Script";
        }

        public Type ScriptType
        {
            get { return scriptType; }
        }

        protected override void OnFileNameUpdated()
        {
            Type oldType = scriptType;

            if (CompileScript())
                CambiarScripts(oldType, scriptType);

            base.OnFileNameUpdated();
        }

        public override void OnFileChanged()
        {
            Type oldType = scriptType;

            if (CompileScript())
                CambiarScripts(oldType, scriptType);

            base.OnFileChanged();
        }

        public override void OnFileDeleted()
        {
            missing = true;

            base.OnFileDeleted();
        }

        private void CambiarScripts(Type oldType, Type newType)
        {
            if (oldType != null)
            {
                foreach (Component comp in new List<Component>(Component.AllComponentsOfType(oldType)))
                {
                    FieldValue[] valores = comp.GetValores();
                    comp.Destroy();
                    Script script = (Script)scriptType.GetConstructor(Type.EmptyTypes).Invoke(null);
                    script.SetValores(valores);
                }
            }
        }

        private bool CompileScript()
        {
            Name = Path.GetFileNameWithoutExtension(FileName);

            missing = false;
            if (File.Exists(FileName))
            {
                using (CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    try
                    {
                        String source = File.ReadAllText(FileName);

                        String exeLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                        CompilerParameters compParameters = new CompilerParameters(new String[] { Path.Combine(exeLocation, "Fang3D.DLL"), Path.Combine(exeLocation, "OpenTK.Utilities.dll"), Path.Combine(exeLocation, "OpenTK.dll") });
                        compParameters.GenerateInMemory = true;

                        CompilerResults result = provider.CompileAssemblyFromSource(compParameters, source);

                        if (result.Errors.HasErrors || result.Errors.HasWarnings)
                        {
                            foreach (CompilerError err in result.Errors)
                            {
                                if (err.IsWarning)
                                    Console.WriteWarning(err.ToString());
                                else
                                    Console.WriteError(err.ToString());
                            }
                        }

                        if (result.Errors.HasErrors == false)
                        {
                            Type internalType = result.CompiledAssembly.GetType(Name);

                            if (internalType != null)
                            {
                                if (typeof(Script).IsAssignableFrom(internalType))
                                {
                                    scriptType = internalType;

                                    Console.WriteLog(FileName + " : Compilación exitosa ");

                                    return true;
                                }
                                else
                                {
                                    Console.WriteError(FileName + " : La clase definida en el archivo no hereda de Script");
                                }
                            }
                            else
                            {
                                Console.WriteError(FileName + " : El nombre del archivo no coincide con el nombre de la clase declarada adentro");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteError("Compilation Error: " + ex.ToString());
                    }
                }
            }

            missing = true;
            return false;
        }

        public override object GetInstance()
        {
            if (scriptType != null)
                return scriptType.GetConstructor(Type.EmptyTypes).Invoke(null);

            return null;
        }

        public override bool CanAssignTo(object obj)
        {
            if (scriptType != null && obj is Entity)
                return true;

            return false;
        }

        public override object AssignTo(object obj)
        {
            if (obj is Entity)
            {
                Script script = (Script) scriptType.GetConstructor(Type.EmptyTypes).Invoke(null);

                script.Entity = (Entity) obj;

                return script;
            }

            return null;
        }
    }
}
