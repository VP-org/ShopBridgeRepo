using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException() : base($"Product not found")
        {
            
        }

        public ProductNotFoundException(int id) : base($"Product id '{id}' not found")
        {

        }
    }

    public class ProductAlreadyExistsException : Exception
    {
        public ProductAlreadyExistsException() : base($"Product already exists")
        {

        }

        public ProductAlreadyExistsException(string name) : base($"Product name '{name}' already exists")
        {

        }
    }
}
