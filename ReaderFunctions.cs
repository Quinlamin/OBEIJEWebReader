using EpubSharp;
using Npgsql;
namespace OBEIJEWebReader;

public static class ReaderFunctions
{
    public class Book
    {
        public int bookID;
        public string bookName;
        public string bookPath;

        public Book(int _bookID, string _bookName, string _bookPath)
        {
            bookID= _bookID;
            bookName =_bookName;
            bookPath = _bookPath;
        }
    }
    public static Book GetBook(int id)
    {
        Book book = null;
        using (var connection = new NpgsqlConnection(EnvironmentVariables.connectionString))
        {
            try
            {
                connection.Open();
                string sql = "SELECT bookid,bookname,bookpath from public.books where bookid = @index";
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("index",id);
                    
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            book = new Book(reader.GetInt32(0),reader.GetString(1),reader.GetString(2));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        return book;
    }
    public static async Task<Book[]> GetBooks()
    {
        List<Book> books =new List<Book>();

        
        using (var connection = new NpgsqlConnection(EnvironmentVariables.connectionString))
        {
            try
            {
                connection.Open();
                string sql = "SELECT bookid,bookname,bookpath FROM public.books";
                using(var cmd = new NpgsqlCommand(sql, connection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);

                            string bookname = reader.GetString(1);
                            string bookpath = reader.GetString(2);
                            books.Add(new Book(id,bookname,bookpath));
                        }
                    }
                }
            }
            catch
            {
                
            }
        }
        return books.ToArray();
    }
    
}