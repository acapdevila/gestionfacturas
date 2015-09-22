namespace GestionFacturas.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Facturas_Pie_800Caracteres : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Facturas", "ComentariosPie", c => c.String(maxLength: 800));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Facturas", "ComentariosPie", c => c.String(maxLength: 500));
        }
    }
}
