using Lab1.Domain.BankServices;
using Lab1.Domain.Repositories;

namespace Lab1.Domain.Users
{
    internal class CompanySpecialist(ICompanySpecialistRepository companySpecialistRepository) : User
    {
        public required string IdNumber { get; set; }
        public required string Name { get; set; }
        public required Company Company { get; set; } // shows what Company CompanySpecialist is related to

        public void AddSalaryProject()
        {
            if (Company.SalaryProject != null)
            {
                Console.WriteLine("\nERROR!\nSalaryProject already exists\n");
                return;
            }

            var salaryProject = new SalaryProject()
            {
                BankId = Company.Account.Bank.Id,
                CompanyName = Company.Name
            };

            Company.SalaryProject = salaryProject;

            var creatingTask = companySpecialistRepository.CreateSalaryProjectAsync(salaryProject, CancellationToken.None);
            creatingTask.Wait();
            var updatingCompanyTask = companySpecialistRepository.UpdateCompanyAsync(Company.Id, salaryProject.IdNumber, CancellationToken.None);
            updatingCompanyTask.Wait();
            
        }

        public void ApproveSalaryProjectRequest(Client client)
        {
            if (Company.SalaryProject == null)
            {
                Console.WriteLine("\nERROR!\nNo SalaryProject for this company!");
                return;
            }

            var readingTask = companySpecialistRepository.ReadSalaryProjectRequestAsync(client.Login, Company.Name, CancellationToken.None);
            readingTask.Wait();
            var salaryProjectRequest = readingTask.Result;

            if (salaryProjectRequest == null)
            {
                Console.WriteLine("\nERROR!\nNo such SalaryProject!\n");
                return;
            }

            if (salaryProjectRequest.IsApproved ==  true)
            {
                Console.WriteLine($"\nClinet {client.Name} has already approved!\n");
                return;
            }

            var approvalRequestTask = companySpecialistRepository.ApproveSalaryProjectRequestAsync(salaryProjectRequest, CancellationToken.None);
            approvalRequestTask.Wait();

            var addingSalaryTask = companySpecialistRepository.AddSalaryAsync(salaryProjectRequest, Company.SalaryProject, CancellationToken.None);
            addingSalaryTask.Wait();
        }

        public void DecompleteSalaryProject()
        {
            if (Company.SalaryProject != null)
            {
                Company.SalaryProject.IsCompleted = false;
                companySpecialistRepository.DecompleteSalaryProjectAsync(Company.SalaryProject, CancellationToken.None).Wait();
                return;
            }
            else
            {
                Console.WriteLine("\nERROR! No salary project!\n");
                return;
            }
        }

        public override string ToString()
        {
            return $"CompanySpecialist: [Login: {Login}, Name: {Name}, IdNumber: {IdNumber}, Company: {Company.Name}]";
        }
    }
}
