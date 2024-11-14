using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attelas.Models;

public class InvoiceConfig : IEntityTypeConfiguration<InvoiceModel>
{
    public void Configure(EntityTypeBuilder<InvoiceModel> builder)
    {
        builder.ToTable("t_invoices");
    }
}