using System;
using System.Collections.Generic;

namespace OnlineLibrarySystem
{
    // Singleton class representing the Library
    public class Library
    {
        private static Library _instance;
        public List<Book> Books { get; private set; }

        // Private constructor for Singleton pattern
        private Library()
        {
            Books = new List<Book>();
        }

        // Method to get the single instance of Library
        public static Library GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Library();
            }
            return _instance;
        }

        // Method to add a book to the library
        public void AddBook(Book book)
        {
            Books.Add(book);
        }

        // Method to display all books in the library
        public void DisplayBooks()
        {
            Console.WriteLine("\nAvailable Books in the Library:");
            foreach (var book in Books)
            {
                Console.WriteLine($"{book.Title} by {book.Author} - Status: {book.State.GetType().Name} - Premium: {book.IsPremium}");
            }
        }
    }

    // Class representing a Book
    public class Book
    {
        public string Title { get; }
        public string Author { get; }
        public IState State { get; set; }
        public bool IsPremium { get; }

        // Constructor for Book class
        public Book(string title, string author, bool isPremium)
        {
            Title = title;
            Author = author;
            IsPremium = isPremium;
            State = new AvailableState(); // Initial state is available
        }

        // Method to borrow the book
        public void Borrow()
        {
            State.Borrow(this);
        }

        // Method to reserve the book
        public void Reserve()
        {
            State.Reserve(this);
        }

        // Method to return the book
        public void ReturnBook()
        {
            State.ReturnBook(this);
        }
    }

    // Interface for State pattern
    public interface IState
    {
        void Borrow(Book book);
        void Reserve(Book book);
        void ReturnBook(Book book);
    }

    // Class for Available State
    public class AvailableState : IState
    {
        public void Borrow(Book book)
        {
            book.State = new BorrowedState();
            Console.WriteLine($"{book.Title} has been borrowed.");
        }

        public void Reserve(Book book)
        {
            book.State = new ReservedState();
            Console.WriteLine($"{book.Title} has been reserved.");
        }

        public void ReturnBook(Book book)
        {
            Console.WriteLine($"{book.Title} is already available.");
        }
    }

    // Class for Borrowed State
    public class BorrowedState : IState
    {
        public void Borrow(Book book)
        {
            Console.WriteLine($"{book.Title} is already borrowed.");
        }

        public void Reserve(Book book)
        {
            Console.WriteLine($"{book.Title} cannot be reserved while borrowed.");
        }

        public void ReturnBook(Book book)
        {
            book.State = new AvailableState();
            Console.WriteLine($"{book.Title} has been returned.");
        }
    }

    // Class for Reserved State
    public class ReservedState : IState
    {
        public void Borrow(Book book)
        {
            Console.WriteLine($"{book.Title} cannot be borrowed while reserved.");
        }

        public void Reserve(Book book)
        {
            Console.WriteLine($"{book.Title} is already reserved.");
        }

        public void ReturnBook(Book book)
        {
            book.State = new AvailableState();
            Console.WriteLine($"{book.Title} has been returned from reservation.");
        }
    }

    // Abstract class representing a User
    public abstract class User
    {
        public string Name { get; }
        public bool IsPremium { get; }

        protected User(string name, bool isPremium)
        {
            Name = name;
            IsPremium = isPremium;
        }

        // Method for borrowing a book
        public void BorrowBook(Book book)
        {
            if (!book.IsPremium || IsPremium)
            {
                book.Borrow();
            }
            else
            {
                Console.WriteLine($"{Name}, you are not allowed to borrow premium books.");
            }
        }

        // Method for reserving a book
        public void ReserveBook(Book book)
        {
            book.Reserve();
        }
    }

    // Class for Premium User
    public class PremiumUser : User
    {
        public PremiumUser(string name) : base(name, true) { }
    }

    // Class for Standard User
    public class StandardUser : User
    {
        public StandardUser(string name) : base(name, false) { }
    }

    // Interface for Iterator pattern
    public interface IIterator<T>
    {
        bool HasNext();
        T Next();
    }

    // Class for iterating through books
    public class BookIterator : IIterator<Book>
    {
        private readonly List<Book> _books;
        private int _index;

        public BookIterator(List<Book> books)
        {
            _books = books;
            _index = 0;
        }

        public bool HasNext()
        {
            return _index < _books.Count;
        }

        public Book Next()
        {
            return _books[_index++];
        }
    }

    // Main program class
    class Program
    {
        static void Main(string[] args)
        {
            // Get the singleton instance of the library
            Library library = Library.GetInstance();

            // Adding books to the library
            library.AddBook(new Book("The Great Gatsby", "F. Scott Fitzgerald", false));
            library.AddBook(new Book("1984", "George Orwell", false));
            library.AddBook(new Book("Moby Dick", "Herman Melville", true)); // Premium book
            library.AddBook(new Book("War and Peace", "Leo Tolstoy", true)); // Premium book

            // Displaying available books
            library.DisplayBooks();

            // Creating users
            User standardUser = new StandardUser("Alice");
            User premiumUser = new PremiumUser("Bob");

            // Standard User tries to borrow a premium book
            standardUser.BorrowBook(library.Books[2]); // Attempt to borrow Moby Dick
            standardUser.BorrowBook(library.Books[0]); // Borrow The Great Gatsby

            // Premium User borrows a book
            premiumUser.BorrowBook(library.Books[2]); // Borrow Moby Dick

            // Displaying available books after borrowing
            library.DisplayBooks();

            // Standard User tries to reserve a book
            standardUser.ReserveBook(library.Books[1]); // Reserve 1984
            standardUser.BorrowBook(library.Books[1]); // Attempt to borrow 1984

            // Premium User returns a book
            premiumUser.BorrowBook(library.Books[2]); // Borrow Moby Dick
            library.Books[2].ReturnBook(); // Return Moby Dick

            // Displaying available books after returning
            library.DisplayBooks();
        }
    }
}
