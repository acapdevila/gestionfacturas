namespace GestionFacturas.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cliente_LongitudDireccion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clientes", "Direccion", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clientes", "Direccion", c => c.String(maxLength: 50));
        }
    }
}
