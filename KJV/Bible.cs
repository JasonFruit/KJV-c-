using System;
using System.Collections.Generic;
using Windows.Data.Json;

namespace KJV
{
    public static class BibleData
    {
        public static List<Book> Books = new List<Book>();

        public static List<string> BookNames = new List<string>();

        private static async void readBibleData()
        {
            Windows.Storage.StorageFolder fld = Windows.ApplicationModel.Package.Current.InstalledLocation;
            Windows.Storage.StorageFile f = await fld.GetFileAsync("bible_data.json");
            string content = await Windows.Storage.FileIO.ReadTextAsync(f);
            JsonArray data = JsonArray.Parse(content);
            foreach (JsonValue val in data)
            {
                JsonObject jBook = val.GetObject();
                Book book = new Book();
                book.Number = (int)jBook.GetNamedNumber("num");
                book.Name = jBook.GetNamedString("name");

                JsonArray chaps = jBook.GetNamedArray("chapters");

                foreach (JsonValue chapVal in chaps)
                {
                    JsonObject jChap = chapVal.GetObject();
                    Chapter chap = new Chapter();
                    chap.Number = (int)jChap.GetNamedNumber("num");
                    chap.Verses = (int)jChap.GetNamedNumber("verses");
                    book.Chapters.Add(chap);
                }
                Books.Add(book);
            }

            foreach (Book book in Books)
            {
                BookNames.Add(book.Name);
            }

        }

        static BibleData()
        {
            readBibleData();
        }
    }
}
