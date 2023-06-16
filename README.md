# LEGO API Demo

I don't have many C# / ASP.Net code samples that are mine to share, and I also need some practice with newer .NET 7 approaches to building web apps. This is a very simple web API project that provides data about the parts found in various LEGO building sets.

The LEGO parts inventory data comes from a snapshot of the data from Rebrickable that was [made available on Kaggle](https://www.kaggle.com/datasets/rtatman/lego-database). I've imported it into a SQLite database that's suitable for running the app locally. If you were to deploy it to a server somewhere, you'd probably want that data on a server, instead.
