using System.Configuration;
using EPE.BusinessLayer;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePathAlunos = @"E:\MegaSync\EPE\EntityFramework\Alunos_2018-2019.xls";

            var adapter = new AlunoFileAdapter(filePathAlunos, ConfigurationManager.ConnectionStrings["EPEValidation"].ConnectionString);

            //adapter.NumberOfRowsToImportDetermined += Adapter_NumberOfRowsToImportDetermined;

            //adapter.RowTreated += Adapter_RowTreated;

            adapter.LoadData();
        }
    }
}
