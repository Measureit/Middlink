using System;
using System.Collections.Generic;
using System.Text;

namespace Middlink.Authentication
{
  public class AuthenticateRequest
  {
    //[Required]
    public string Username { get; set; }

    //[Required]
    public string Password { get; set; }
  }
}
