using System;

namespace Project.Models.Exceptions;

public class AccountBannedException : Exception
{
    public AccountBannedException() : base("Your account has been suspended.") { }
}
