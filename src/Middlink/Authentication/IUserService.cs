using System;
using System.Collections.Generic;
using System.Text;

namespace Middlink.Authentication
{
  public interface IUserService
  {
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    IEnumerable<User> GetAll();
  }
}
