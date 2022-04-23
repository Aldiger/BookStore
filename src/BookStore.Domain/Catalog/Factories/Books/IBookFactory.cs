﻿namespace BookStore.Domain.Catalog.Factories.Books;

using Common;
using Models.Authors;
using Models.Books;

public interface IBookFactory : IFactory<Book>
{
    IBookFactory WithTitle(string title);

    IBookFactory WithPrice(decimal price);

    IBookFactory WithGenre(Genre genre);

    IBookFactory FromAuthor(Author author);
}