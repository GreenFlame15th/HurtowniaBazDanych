using System;
using System.Collections.Generic;
using System.Text;

namespace HurtowniaBazDanych
{
    class StringDataObjectList : List<StringDataObject>
    {
        public dataType[] signature;

        public StringDataObjectList(params dataType[] signature)
        {
            this.signature = signature;
        }
    }

}
