using System;
public class Assertion 
{
    public class AssertionException : Exception
    {
        public AssertionException(string message) : base(message) { }
    }

    public static void NotNull(object target)
    {
        NotNull(target, "Expected target to be not null");
    }

    public static void NotNull(object target, string message)
    {
        if (target == null)
        {
            throw new AssertionException(message);
        }
    }
}
