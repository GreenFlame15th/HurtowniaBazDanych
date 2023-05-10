using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace HurtowniaBazDanych
{
    class Program
    {
        static void Main(string[] args)
        {
            string spacer = ","; //string bettwen each element of an item
            string lineSpacer = "),\n("; //string bettwen each element
            int recordBlockCount = 5; //number of chunks for salesRecords
            int recordChunkSize = 2000; //size of each chunk (recordBlockCount * recordChunkSize = total sells)
            int[] saleWeiths = {20, 10, 3}; //randome weights used to generate rnadome numver of records sold
            string nullString = "NULL"; // string for null entry
            string into = "INSERT INTO ";
            string values = "\nVALUES(";
            string saFix = ");";
            #region defineLists
            Console.WriteLine("Making base data");
            List<Date> dateList = Date.GenerateDatesYears(DateTime.Today, 3);
            StringDataObjectList authorList = new StringDataObjectList(dataType.STR, dataType.STR, dataType.STR);
            StringDataObjectList bookList = new StringDataObjectList(dataType.INT, dataType.INT, dataType.STR);
            StringDataObjectList payMethodList = new StringDataObjectList(dataType.STR);
            List<Promo> saleList = new List<Promo>();
            StringDataObjectList receivingPlaceList = new StringDataObjectList(dataType.STR, dataType.STR);
            StringDataObjectList lokalList = new StringDataObjectList(dataType.STR);
            StringDataObjectList genreList = new StringDataObjectList(dataType.STR);
            StringDataObjectList seleRecordList = new StringDataObjectList(dataType.INT, dataType.INT, dataType.INT, dataType.INT, dataType.INT, dataType.INT, dataType.INT, dataType.INT);
            StringDataObjectList recivreMethodList = new StringDataObjectList(dataType.STR);
            #endregion
            #region defieData
            //makes locals
            lokalList.Add(new StringDataObject("local", "Insrnet"));
            lokalList.Add(new StringDataObject("local", "Warszawa"));
            lokalList.Add(new StringDataObject("local", "Warszawa"));
            lokalList.Add(new StringDataObject("local", "Krakow"));
            lokalList.Add(new StringDataObject("local", "Ludzi"));

            //makes genre
            genreList.Add(new StringDataObject("genre", "Fantastyka")); //0
            genreList.Add(new StringDataObject("genre", "Literatura tradycyjna")); //1
            genreList.Add(new StringDataObject("genre", "Naukowe")); //2
            //lokalList.Add(new StringDataObject("genre", "Fikcja historyczna"));
            //lokalList.Add(new StringDataObject("genre", "Fantastyka naukowa")); 
            //lokalList.Add(new StringDataObject("genre", "Kryminal"));
            //lokalList.Add(new StringDataObject("genre", "Humor"));
            //lokalList.Add(new StringDataObject("genre", "Poezja"));
            //lokalList.Add(new StringDataObject("genre", "Romans"));
            //lokalList.Add(new StringDataObject("genre", "Fikcja realistyczna")); 

            //makes autors and books
            authorList.Add(new StringDataObject("autor", "Adam","Mickiewicz","")); //0
            for (int i = 0; i < 12; i++)
            {
                bookList.Add(new StringDataObject("book", "1", "0", "Dziady " + (1+i)));
            }

            authorList.Add(new StringDataObject("autor", "", "", "Homer")); //1
            bookList.Add(new StringDataObject("book", "1", "1", "Iliada"));
            bookList.Add(new StringDataObject("book", "1", "1", "Odyseja "));

            authorList.Add(new StringDataObject("autor", "John", "Flanagan", "")); //2
            for (int i = 0; i < 9; i++)
            {
                bookList.Add(new StringDataObject("book", "0", "2", "Druzyna " + (1 + i)));
            }
            for (int i = 0; i < 17; i++)
            {
                bookList.Add(new StringDataObject("book", "1", "2", "Zwiadowcy " + (1 + i)));
            }

            authorList.Add(new StringDataObject("autor", "Ian", "Stewart", "")); //3
            bookList.Add(new StringDataObject("book", "3", "2", "Krowy w labiryncie i inne eksploracje matematyczne"));
            bookList.Add(new StringDataObject("book", "3", "2", "Gabinet matematycznych zagadek 1"));
            bookList.Add(new StringDataObject("book", "3", "2", "Gabinet matematycznych zagadek 2"));
            bookList.Add(new StringDataObject("book", "3", "2", "Po co nam matematyka. Niedorzeczna skuteczność matematykik"));


            //makes payment methods
            payMethodList.Add(new StringDataObject("payMethod", "Cash"));
            payMethodList.Add(new StringDataObject("payMethod", "Card"));
            payMethodList.Add(new StringDataObject("payMethod", "Blik"));
            payMethodList.Add(new StringDataObject("payMethod", "Gift Card"));

            //makes payment methods
            RandomeGeneratorWithWeithtedDistrubusion bookRandSale = new RandomeGeneratorWithWeithtedDistrubusion(bookList.Count, 10);

            saleList.Add(new Promo("sale", "70",30, dateList.Count));
            saleList.Add(new Promo("sale", "50",30, dateList.Count));
            saleList.Add(new Promo("sale", "50",1, dateList.Count));
            saleList.Add(new Promo("sale", "30",30, dateList.Count));
            saleList.Add(new Promo("sale", "30",90, dateList.Count));
            saleList.Add(new Promo("sale", "30",2, dateList.Count));

            //makes receivingPlace
            string[] postCode = { "05075", "11111", "10120", "10130", "01001", "50050", "66466", "75173", "24343", "23002", "12001", "75690", "60500" };
            for (int i = 0; i < postCode.Length; i++)
            {
                receivingPlaceList.Add(new StringDataObject("receivingPlace", "Insrnet", postCode[i]));
                receivingPlaceList.Add(new StringDataObject("receivingPlace", "Warszawa", postCode[i]));
                receivingPlaceList.Add(new StringDataObject("receivingPlace", "Krakow", postCode[i]));
                receivingPlaceList.Add(new StringDataObject("receivingPlace", "Ludzi", postCode[i]));
            }

            //add revice method
            recivreMethodList.Add(new StringDataObject("reviceMethod", "W lokacji"));
            recivreMethodList.Add(new StringDataObject("reviceMethod", "Kurier"));
            recivreMethodList.Add(new StringDataObject("reviceMethod", "Paczkomat"));
            #endregion
            #region MakingrnadomeDiscribusion
            for (int i = 0; i < recordBlockCount; i++)
            {
                Console.WriteLine("Chunk [" + (i+1) + " / " + recordBlockCount + "]");
                Console.WriteLine("Making rnadomeDiscribusion");
                RandomeGeneratorWithWeithtedDistrubusion bookRand = new RandomeGeneratorWithWeithtedDistrubusion(bookList.Count, 20, 11);
                RandomeGeneratorWithWeithtedDistrubusion payMethodRand = new RandomeGeneratorWithWeithtedDistrubusion(payMethodList.Count, 7);
                RandomeGeneratorWithWeithtedDistrubusion receivingPlaceRand = new RandomeGeneratorWithWeithtedDistrubusion(receivingPlaceList.Count, 50);
                RandomeGeneratorWithWeithtedDistrubusion lokalRand = new RandomeGeneratorWithWeithtedDistrubusion(lokalList.Count, 5);
                RandomeGeneratorWithWeithtedDistrubusion sellCountRand = new RandomeGeneratorWithWeithtedDistrubusion(saleWeiths);
                RandomeGeneratorWithWeithtedDistrubusion dateRand = new RandomeGeneratorWithWeithtedDistrubusion(dateList.Count, 100, 30);
                RandomeGeneratorWithWeithtedDistrubusion recivreMethodRand = new RandomeGeneratorWithWeithtedDistrubusion(recivreMethodList.Count, 10);

                #endregion
            #region MakingSelesRecords
                Console.WriteLine("Making seles records");
                for (int j = 0; j < recordChunkSize; j++)
                {
                    int loc = lokalRand.next();
                    int dateId = dateRand.next();
                    string promoStr = saleList.findPromoString(dateId, nullString);
                    

                    if (loc != 1)
                    {
                        seleRecordList.Add(new StringDataObject("seleRecord", sellCountRand.next() + "", dateId + "", loc + "", bookRand.next() + "", payMethodRand.next() + "", promoStr, "0", nullString));
                    }
                    else
                    {
                        int reciveMethod = recivreMethodRand.next();
                        if(reciveMethod == 1)
                        {
                            seleRecordList.Add(new StringDataObject("seleRecord", sellCountRand.next() + "", dateId + "", loc + "", bookRand.next() + "", payMethodRand.next() + "", promoStr, reciveMethod+"", nullString));
                        }
                        else
                        {
                            seleRecordList.Add(new StringDataObject("seleRecord", sellCountRand.next() + "", dateId + "", loc + "", bookRand.next() + "", payMethodRand.next() + "", promoStr, reciveMethod + "", receivingPlaceRand.next()+""));
                        }
                    }

                }
            }
            #endregion
   
            #region fileAppending
            Console.WriteLine("File appending");
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            List<string> statments = new List<string>();

            dateList.WriteToFile(statments, spacer, lineSpacer, projectDirectory, "date", into + "date" + values, saFix);
            lokalList.WriteToFile(statments, spacer, lineSpacer, projectDirectory, "lokal", into + "local" + values, saFix);
            genreList.WriteToFile(statments, spacer, lineSpacer, projectDirectory, "genre", into + "genre" + values, saFix);
            authorList.WriteToFile(statments, spacer, lineSpacer, projectDirectory, "autor", into + "author" + values, saFix);
            bookList.WriteToFile(statments, spacer, lineSpacer, projectDirectory, "book", into + "book" + values, saFix);
            payMethodList.WriteToFile(statments, spacer, lineSpacer, projectDirectory, "payMethod", into + "payment_method" + values, saFix);
            saleList.WriteToFile(statments, spacer, lineSpacer, projectDirectory, "Sale", into + "sale" + values, saFix); 
            receivingPlaceList.WriteToFile(statments, spacer, lineSpacer, projectDirectory, "receivingPlace", into + "delivery_place" + values, saFix);
            recivreMethodList.WriteToFile(statments, spacer, lineSpacer, projectDirectory, "recivreMethod", into + "delivery_method" + values, saFix); 
            seleRecordList.WriteToFile(statments, spacer, lineSpacer, projectDirectory, "saleRecord", into + "sale_record" + values, saFix); 

            #endregion
            #region SQL

            using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\ProjectsV13;Initial Catalog=HurtowniaBazDanych;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                connection.Open();

                Console.WriteLine("SQL delate");
                Action<string> delete = (table) => {
                    try
                    {
                        Console.WriteLine(table);
                        SqlCommand command = new SqlCommand(
                        "DELETE FROM "+table, connection);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                };

                string[] tables = { "sale_record", "book", "date", "local", "genre", "author", "payment_method", "sale", "delivery_place", "delivery_method" };

                foreach (String table  in tables )
                {
                    try
                    {
                        delete(table);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                
                Console.WriteLine("SQL insert");

                for (int i = 0; i < statments.Count; i++)
                {
                    try
                    {
                        Console.WriteLine(i);
                        SqlCommand command = new SqlCommand(
                        statments[i], connection);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(statments[i]);
                        Console.WriteLine(e);
                    }
                }
            }

            #endregion
        }


    }
}
