using System;
using System.Security.Claims;

namespace AlphaDev.Security
{
    public interface ISecurityTokenWriter
    {
        string Generate(Claim[] claims);
    }
}