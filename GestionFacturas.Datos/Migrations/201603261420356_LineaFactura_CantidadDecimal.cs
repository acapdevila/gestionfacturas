namespace GestionFacturas.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LineaFactura_CantidadDecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Facturas", "VendedorDireccion", c => c.String(maxLength: 128));
            AlterColumn("dbo.Facturas", "CompradorDireccion", c => c.String(maxLength: 128));
            AlterColumn("dbo.FacturasLineas", "Cantidad", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FacturasLineas", "Cantidad", c => c.Int(nullable: false));
            AlterColumn("dbo.Facturas", "CompradorDireccion", c => c.String(maxLength: 50));
            AlterColumn("dbo.Facturas", "VendedorDireccion", c => c.String(maxLength: 50));
        }
    }
}
