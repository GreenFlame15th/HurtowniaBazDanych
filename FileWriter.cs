using HurtowniaBazDanych;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

static class FileWriter
{
    public static void WriteToFile<T>(this IList<T> list, List<string> commands, string spacer, string lineSpacer, string dir, string fileName, string prefix, string safix, int startpoint = 0) where T : HurtowniaBazDanych.DataObject
    {

        string filename2 = dir + "\\OutPut\\" + fileName + ".txt";
        string directoryName = Path.GetDirectoryName(filename2);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        int endPoint = list.Count - 1;

        if (startpoint == 0)
        {
            Console.WriteLine(filename2);
            File.WriteAllText(filename2, "");
        }

        if(endPoint-startpoint > 500)
        {
            endPoint = startpoint + 500;
            list.WriteToFile(commands, spacer, lineSpacer, dir, fileName, prefix, safix, startpoint + 501);
        }

        StringBuilder sb = new StringBuilder(prefix);

        for (int i = startpoint; i < endPoint; i++)
        {
            sb.Append(list[i].ToString(spacer));
            sb.Append(lineSpacer);
        }
        sb.Append(list[endPoint].ToString(spacer));
        sb.Append(safix);

        string toRetun = sb.ToString();
        File.AppendAllText(filename2, toRetun);
        File.AppendAllText(filename2, "\n");
        commands.Add(toRetun);
    }

    public static void WriteToFile(this HurtowniaBazDanych.StringDataObjectList list, List<string> commands, string spacer, string lineSpacer, string dir, string fileName, string prefix, string safix, int startpoint = 0)
    {
        string filename2 = dir + "\\OutPut\\" + fileName + ".txt";
        string directoryName = Path.GetDirectoryName(filename2);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }


        int endPoint = list.Count - 1;
        if (startpoint == 0)
        {
            Console.WriteLine(filename2);
            File.WriteAllText(filename2, "");
        }

        if (endPoint - startpoint > 500)
        {
            endPoint = startpoint + 500;
            list.WriteToFile(commands, spacer, lineSpacer, dir, fileName, prefix, safix, startpoint + 501);
        }

        StringBuilder sb = new StringBuilder(prefix);

        for (int i = startpoint; i < endPoint; i++)
        {
            sb.Append(list[i].ToString(list.signature, spacer));
            sb.Append(lineSpacer);
        }
        sb.Append(list[endPoint].ToString(list.signature, spacer));

        sb.Append(safix);

        string toRetun = sb.ToString();
        File.AppendAllText(filename2, toRetun);
        File.AppendAllText(filename2, "\n");
        commands.Add(toRetun);
    }

}