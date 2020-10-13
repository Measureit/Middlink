using System;
using System.Collections.Generic;
using System.Text;
using Middlink.Messages.Entities;

namespace Middlink.Entities
{
    public class IdentityEntity : IIdentifiable
    {
        public Guid Id { get; set; }
        public string ProviderUserId { get; set; }
        public string Issuer { get; set; }

        public IdentityEntity(Guid id, string providerId, string issuer)
        {
            Id = id;
            ProviderUserId = providerId;
            Issuer = issuer;
        }
    }
}
