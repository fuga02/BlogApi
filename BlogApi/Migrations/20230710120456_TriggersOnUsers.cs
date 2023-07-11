using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Migrations
{
    /// <inheritdoc />
    public partial class TriggersOnUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //create materialized view like cache
            migrationBuilder.Sql("CREATE  MATERIALIZED VIEW users_data AS SELECT * FROM \"Users\" WITH DATA;");

            //create a trigger for when user updates, this trigger refresh view
            migrationBuilder.Sql("CREATE or REPLACE FUNCTION on_update_user()\r\nRETURNS TRIGGER\r\nLANGUAGE PLPGSQL\r\nAS\r\n$$\r\nBEGIN\r\n    REFRESH MATERIALIZED VIEW users;\r\n    RETURN NEW ;\r\nEND\r\n$$;");
            migrationBuilder.Sql("CREATE TRIGGER on_update_users_trigger\r\nAFTER UPDATE ON \"Users\"\r\nFOR EACH ROW\r\nEXECUTE FUNCTION on_update_user();");


            //create cache table for deleted columns on users table
            migrationBuilder.Sql("CREATE TABLE backup_users_table AS SELECT * FROM  \"Users\" ;");
            migrationBuilder.Sql("DELETE FROM backup_users_table;");

            //create trigger for saving deleting columns
            migrationBuilder.Sql("CREATE OR REPLACE FUNCTION on_user_delete()\r\nRETURNS TRIGGER\r\nLANGUAGE PLPGSQL\r\nAS $$\r\nBEGIN\r\n    INSERT INTO backup_users_table(\"Id\", \"Name\", \"Username\", \"PasswordHash\")\r\n    VALUES (OLD.\"Id\", OLD.\"Name\", OLD.\"Username\", OLD.\"PasswordHash\");\r\n    RETURN OLD;\r\nEND;\r\n$$;");
            migrationBuilder.Sql("\r\nCREATE Trigger on_users_delete_trigger\r\nBEFORE DELETE\r\nON \"Users\"\r\nFOR EACH ROW\r\nEXECUTE PROCEDURE on_user_delete();");

            //
            migrationBuilder.Sql("CREATE Table user_history(\r\n    id SERIAL PRIMARY KEY,\r\n    user_id uuid,\r\n    old_name TEXT,\r\n    new_name TEXT,\r\n    old_username TEXT,\r\n    new_username TEXT,\r\n    date_time TIMESTAMP\r\n);");
            migrationBuilder.Sql("CREATE or REPLACE FUNCTION on_user_update()\r\nRETURNS TRIGGER\r\nLANGUAGE PLPGSQL\r\nAS\r\n$$\r\nBEGIN\r\n    INSERT INTO user_history(user_id,old_name,new_name,old_username,new_username,date_time)\r\n    VALUES(OLD.Id,OLD.NAME,NEW.NAME,OLD.Username,NEW.Username,now());\r\n    RETURN NEW;\r\nEND\r\n$$;");
            migrationBuilder.Sql("CREATE Trigger on_users_update_trigger\r\nAFTER UPDATE\r\nON \"Users\"\r\nfor EACH ROW\r\nEXECUTE PROCEDURE on_user_update();");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
