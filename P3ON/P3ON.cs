using Microsoft.VisualBasic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace P3ONCore{
    public class P3ONObj{
        public string Name {get;}
        public string Type {get;}
        public object Value = string.Empty;
        public List<P3ONObj> Children {get;}
        public P3ONObj(string name, string type){
            Name = name;
            Type = type;
            Children = new List<P3ONObj>();
        }

        public override string ToString()
        {
            return $"Name: {Name}, Type: {Type}, Value: {Value}";
        }

        

    }

    public static class P3ON{
        public static Dictionary<string, object> Read(string filepath){
            Dictionary<string, object> TokenData = new Dictionary<string, object>();
            if(File.Exists(filepath)){
                List<string> lines = File.ReadAllLines(filepath).ToList();
                bool objectBeingCreated = false;
                string objectName = ""; 
                bool isNested = false;
                string nestedName = "";
                bool isNotContainer = false;
                string explicitType = "string";
                int lineNumber = 1;
                Console.WriteLine($"Lines read: {lines.Count}");
                foreach(string line in lines){
                    if(isNested){
                        if(line.Trim().Contains("},")){
                            isNested = true;
                        } else if(line.Trim().Contains("}")){
                            isNested = false;
                        }
                    }
                    Console.WriteLine($"Line: {line}, objectBeingCreated: {objectBeingCreated}");
                    if(line.Contains("Obj")){
                        objectBeingCreated = true;
                    } else if(objectBeingCreated == true && line.Contains("&name")){
                        int pos = line.IndexOf("="); 
                         objectName = line.Substring(pos + 2).Replace(",", string.Empty).Replace("'", string.Empty);
                        Console.WriteLine(objectName);
                    } else if(objectBeingCreated == true && line.Contains("&typ")){
                        int pos = line.IndexOf('=');
                        string type = line.Substring(pos + 2).Replace(",", string.Empty).Replace("'", string.Empty);
                        Console.WriteLine("" + type);
                        switch(type){
                            case "string":
                                if(isNested){
                                    ((Dictionary<string, object>)TokenData[nestedName]).Add(objectName, "");
                                    Console.WriteLine("" + objectName);
                                    isNotContainer = true; 
                                } else {
                                    TokenData.Add(objectName, "");
                                    Console.WriteLine("" + objectName);
                                    isNotContainer = true;
                                }
                                break;
                            case "int":
                                if(isNested){
                                    ((Dictionary<string, object>)TokenData[nestedName]).Add(objectName, 0);
                                    Console.WriteLine("" + objectName);
                                    isNotContainer = true; 
                                } else {
                                    TokenData.Add(objectName, 0);
                                    Console.WriteLine("" + objectName);
                                    isNotContainer = true;
                                }
                                break;
                            case "bool":
                                if(isNested){
                                    ((Dictionary<string, object>)TokenData[nestedName]).Add(objectName, true);
                                    Console.WriteLine("" + objectName);
                                    isNotContainer = true; 
                                } else {
                                    TokenData.Add(objectName, true);
                                    Console.WriteLine("" + objectName);
                                    isNotContainer = true;
                                }
                                isNotContainer = true;
                                break;
                            case "binary":
                                if(isNested){
                                    ((Dictionary<string, object>)TokenData[nestedName]).Add(objectName, 0);
                                    Console.WriteLine("" + objectName);
                                    isNotContainer = true; 
                                } else {
                                    TokenData.Add(objectName, 0);
                                    Console.WriteLine("" + objectName);
                                    isNotContainer = true;
                                }
                                break;
                            case "arr.string": 
                                if(isNested){
                                    ((Dictionary<string, object>)TokenData[nestedName]).Add(objectName, new List<string>());
                                    Console.WriteLine("" + objectName);
                                    isNotContainer = true; 
                                } else {
                                    Console.WriteLine("" + objectName);
                                    isNotContainer = true;
                                }
                                break;
                            case "arr.int":
                                if(isNested){
                                    ((Dictionary<string, object>)TokenData[nestedName]).Add(objectName, new List<int>());
                                    Console.WriteLine("" + objectName);
                                    isNotContainer = true; 
                                } else {
                                    Console.WriteLine("" + objectName);
                                    isNotContainer = true;
                                }
                                break;
                            case "arr.bool":
                                if(isNested){
                                    ((Dictionary<string, object>)TokenData[nestedName]).Add(objectName, new List<string>());
                                    Console.WriteLine("" + objectName);
                                    isNotContainer = true; 
                                } else {
                                    Console.WriteLine("" + objectName);
                                    isNotContainer = true;
                                }
                                break;
                            case "container":
                                TokenData.Add(objectName, new Dictionary<string, object>());
                                Console.WriteLine("" + objectName);
                                isNotContainer = false;
                                nestedName = objectName;
                                break;
                        }
                        explicitType = type;
                    } else if(line.Contains("&child")){
                        objectBeingCreated = true;
                        isNested = true;
                    } else if(isNotContainer && line.Contains("&value")){
                        int pos = line.IndexOf("=");
                        object value = "";
                        string value2 = "";
                        if(explicitType == "string"){
                            value = line.Substring(pos + 2).Replace("'", string.Empty);
                            
                        } else if(explicitType.StartsWith("arr") && !isNested){
                            value2 = line.Substring(pos + 2).Replace("]", string.Empty);
                            List<string> arrayReturns = value2.Split(',').ToList();
                            switch(explicitType){
                                case "arr.string":
                                    TokenData[objectName] = arrayReturns;
                                    break;
                                case "arr.int":
                                    TokenData[objectName] = arrayReturns.Select(int.Parse).ToList();
                                    break;
                                case "arr.bool":
                                    TokenData[objectName] = arrayReturns.Select(bool.Parse).ToList();
                                    break;
                            }
                            objectBeingCreated = false;
                        } else if(explicitType.StartsWith("arr") && isNested){
                            value2 = line.Substring(pos + 2).Replace("],", string.Empty).Replace("]", string.Empty);
                            List<string> arrayReturns = value2.Split(',').ToList();
                            switch(explicitType){
                                case "arr.string":
                                    ((Dictionary<string, object>)TokenData[nestedName])[objectName] = arrayReturns;
                                    break;
                                case "arr.int":
                                     ((Dictionary<string, object>)TokenData[nestedName])[objectName] = arrayReturns.Select(int.Parse).ToList();
                                    break;
                                case "arr.bool":
                                     ((Dictionary<string, object>)TokenData[nestedName])[objectName] = arrayReturns.Select(bool.Parse).ToList();
                                    break;
                            }
                        }
                    } 

                    lineNumber++;
                }
            } else {
                throw new NullReferenceException("File at " + filepath + " either does not exist or is being referenced incorrectly" );
            }

            return TokenData;
        }

        public static void Write(Dictionary<string, object> data, string filepath)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("    TokenData: {");

            WriteObject(data, sb, 2, false);

            sb.AppendLine("    }");
            sb.AppendLine("}");

            File.WriteAllText(filepath, sb.ToString());
        }

        private static void WriteObject(Dictionary<string, object> obj, StringBuilder sb, int indentLevel, bool isNested)
        {
            int ln = 0;
            foreach (var kvp in obj)
            {
                ln++;
                string indent = new string(' ', indentLevel * 4);

                if(!isNested) sb.AppendLine($"{indent}Obj{{");
                else if(isNested && ln != 1) sb.AppendLine($"{indent}&child{{");
                sb.AppendLine($"{indent}    &name='{kvp.Key}',");

                if (kvp.Value is Dictionary<string, object> childObj)
                {
                    sb.AppendLine($"{indent}    &typ='container',");
                    sb.AppendLine($"{indent}    &child{{");
                    WriteObject(childObj, sb, indentLevel + 1, true);
                    sb.AppendLine($"{indent}    }},");
                }
                else if (kvp.Value is List<string> stringList)
                {
                    sb.AppendLine($"{indent}    &typ='arr.string',");
                    sb.AppendLine($"{indent}    &value=['{string.Join("', '", stringList)}'],");
                }
                else if (kvp.Value is List<int> intList)
                {
                    sb.AppendLine($"{indent}    &typ='arr.int',");
                    sb.AppendLine($"{indent}    &value=[{string.Join(", ", intList)}],");
                }
                else if (kvp.Value is List<bool> boolList)
                {
                    sb.AppendLine($"{indent}    &typ='arr.bool',");
                    sb.AppendLine($"{indent}    &value=[{string.Join(", ", boolList.Select(b => b.ToString().ToLower()))}],");
                }
                else if (kvp.Value is string stringValue)
                {
                    sb.AppendLine($"{indent}    &typ='string',");
                    sb.AppendLine($"{indent}    &value='{stringValue}',");
                }
                else if (kvp.Value is int intValue)
                {
                    sb.AppendLine($"{indent}    &typ='int',");
                    sb.AppendLine($"{indent}    &value={intValue},");
                }
                else if (kvp.Value is bool boolValue)
                {
                    sb.AppendLine($"{indent}    &typ='bool',");
                    sb.AppendLine($"{indent}    &value={boolValue.ToString().ToLower()},");
                }
                else if (kvp.Value is byte[] byteArrayValue)
                {
                    sb.AppendLine($"{indent}    &typ='string.binary',");
                    sb.AppendLine($"{indent}    &value=[{string.Join(", ", byteArrayValue)}],");
                }

                sb.AppendLine($"{indent}}},");
            }
        }
    }
}