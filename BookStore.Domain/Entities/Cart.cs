﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public void AddItem(Book book, int quantity)
        {
            CartLine line = lineCollection
            .Where(p => p.Book.BookID == book.BookID)
            .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Book = book,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public void RemoveLine(Book book)
        {
            lineCollection.RemoveAll(l => l.Book.BookID == book.BookID);
        }
        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Book.Price * e.Quantity);
        }
        public void Clear()
        {
            lineCollection.Clear();
        }
        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }
    public class CartLine
    {
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }

}
