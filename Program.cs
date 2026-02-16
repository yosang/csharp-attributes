using System;

namespace AttributesExample
{
    // We tag our Custom Attribute with the AttributeUsage attribute.
    // AttributeTargets.All - Can be applied to anything, has no limitations
    // AllowMultiple - Can be used multiple times
    // Inherited - Will be inherited by the derived classes of whatever class it was used on
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class MyCustomAttribute : Attribute // A custom attribute must be inherited by System.Attribute
    {
        // Properties that allows read/write access, theyre set by the constructor
        public string Name { get; set; }
        public string Action { get; set; }

        // The constructor sets the fields when instantiated through the properties
        public MyCustomAttribute(string n, string a)
        {
            Name = n;
            Action = a;
        }

    }

    public class Animal
    {
        // Notice we didnt have to write MyCustomAttribute, C# removes "Attribute" to make it easier for us.
        [MyCustom("Accessor", "Sets / Gets the name of the animal")]
        public string Name { get; set; }
        [MyCustom("Accessor", "Sets / Gets the age of the animal")]
        public int Age { get; set; }

        public Animal(string n, int a)
        {
            Name = n;
            Age = a;
        }

        // A simple demonstration of the attribute Obsolete
        // string message - The message to show when attempting access to a depracated method
        // bool error - if set to false, it will crash the compiler with an error to prevent usage
        // if false, it'll run with a warning
        [Obsolete("Do not use, use the new implementation instead", true)]
        public void EatOld()
        {
            Console.WriteLine("The animal eats");
        }

        public void Eat()
        {
            Console.WriteLine($"{Name} eats");
        }
    }

    public class App
    {
        public static void Main()
        {
            Animal myPet = new("Ella", 3);

            Console.WriteLine(myPet.Name);
            Console.WriteLine(myPet.Age);

            myPet.Name = "Buddy";
            Console.WriteLine(myPet.Name);

            myPet.Eat();

            // Throws an error if we try to use it, because error is set to true, if set to false, it will allow usage with a warning
            // myPet.EatOld();

            // Reflection
            // The art of getting metadata from attributes
            Type type = typeof(Animal);
            Console.WriteLine(type); // AttributesExample.Animal

            // Gets the properties of the class Animal
            foreach (var item in type.GetProperties())
            {
                // Gets the custom attributes used in the Animal class
                foreach (MyCustomAttribute attribute in item.GetCustomAttributes(typeof(MyCustomAttribute), true))
                {
                    Console.WriteLine(item.Name); // Name / Age
                    Console.WriteLine(attribute.Name); // Accessor
                    Console.WriteLine(attribute.Action); // What it does
                }
            }

            // We also used the ObsoleteAttribute, here is how we get the metadata
            foreach (var m in type.GetMethods())
            {
                foreach (ObsoleteAttribute obs in m.GetCustomAttributes(typeof(ObsoleteAttribute), true))
                {
                    if (obs != null)
                    {
                        Console.WriteLine(m.Name);
                        Console.WriteLine(obs.Message);
                        Console.WriteLine(obs.IsError);
                    }
                }
            }
        }
    }


}