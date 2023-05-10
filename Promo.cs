using System;
using System.Collections.Generic;
using System.Text;

namespace HurtowniaBazDanych
{
    class Promo : DataObject
    {
        public string procent;
        public int days;
        public int start;

        public Promo(string siganture,  string procent, int days, int dayCount) : base(siganture)
        {
            this.procent = procent;
            this.days = days;
            start = ThreadSafeRandom.ThisThreadsRandom.Next(dayCount - days); 
        }

        public override string ToString(string spacer = "")
        {
            return id + spacer + procent + spacer + days;
        }
    }

    static class ListPromoExtension
    {
        public static Promo findPromo(this List<Promo> promos, int day)
        {
            return promos.Find((p) => p.start <= day && (p.start + p.days) > day);
        }

        public static String findPromoString(this List<Promo> promos, int day, String nullString)
        {
            Promo promo = promos.findPromo(day);
            if (promo == null)
                return nullString;
            else
                return promo.id + "";
        }
    }
}
