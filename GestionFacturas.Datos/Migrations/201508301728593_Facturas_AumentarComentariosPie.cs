namespace GestionFacturas.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Facturas_AumentarComentariosPie : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Facturas", "VendedorProvincia", c => c.String(maxLength: 50));
            AlterColumn("dbo.Facturas", "VendedorCodigoPostal", c => c.String(maxLength: 10));
            AlterColumn("dbo.Facturas", "ComentariosPie", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Facturas", "ComentariosPie", c => c.String(maxLength: 250));
            AlterColumn("dbo.Facturas", "VendedorCodigoPostal", c => c.String(maxLength: 50));
            AlterColumn("dbo.Facturas", "VendedorProvincia", c => c.String(maxLength: 10));
        }
    }
}
