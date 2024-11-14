using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attelas.Models;

public class ClientConfig : IEntityTypeConfiguration<ClientModel>
{
    public void Configure(EntityTypeBuilder<ClientModel> builder)
    {
        builder.ToTable("t_clients");
    }
}
