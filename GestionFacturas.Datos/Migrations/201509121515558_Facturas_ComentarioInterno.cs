namespace GestionFacturas.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Facturas_ComentarioInterno : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Facturas", "ComentarioInterno", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Facturas", "ComentarioInterno");
        }
    }
}
