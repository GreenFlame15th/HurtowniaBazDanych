using System;
using System.Collections.Generic;
using System.Text;

namespace HurtowniaBazDanych
{
    class StringDataObject : DataObject
    {
        private string[] values;
        public StringDataObject(string signature, params string[] values): base(signature)
        {
            this.values = values;
        }

        public string ToString(dataType[] dataSignature, string spacer = "")
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(id);
            for (int i = 0; i < values.Length; i++)
            {
                if(values[i] == null)
                {
                    sb.Append(spacer + "NULL");
                }
                else
                {
                    switch (dataSignature[i])
                    {
                        case dataType.STR:
                            sb.Append(spacer + "'" + values[i] + "'");
                            break;
                        case dataType.INT:
                            sb.Append(spacer + values[i]);
                            break;
                        default:
                            Console.WriteLine("could not append " + values[i] + " of type " + dataSignature[i]);
                            break;
                    }
                }
            }

            return sb.ToString();
        }

        public override string ToString(string spacer = "")
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(id);
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == null)
                {
                    sb.Append(spacer + "NULL");
                }
                else
                {
                    sb.Append(spacer + values[i]);
                }
            }
            return sb.ToString();
        }
    }
}
