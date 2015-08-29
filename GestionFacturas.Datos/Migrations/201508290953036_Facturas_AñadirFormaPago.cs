namespace GestionFacturas.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Facturas_AñadirFormaPago : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Facturas", "FormaPago", c => c.Int(nullable: false));
            AddColumn("dbo.Facturas", "FormaPagoDetalles", c => c.String(maxLength: 50));
            AlterColumn("dbo.Facturas", "FechaVencimientoFactura", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Facturas", "FechaVencimientoFactura", c => c.DateTime(nullable: false));
            DropColumn("dbo.Facturas", "FormaPagoDetalles");
            DropColumn("dbo.Facturas", "FormaPago");
        }
    }
}
