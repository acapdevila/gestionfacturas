namespace GestionFacturas.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Factura_QuitarCampoLogo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Facturas", "NombreArchivoLogo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Facturas", "NombreArchivoLogo", c => c.String(maxLength: 50));
        }
    }
}
