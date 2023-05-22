The Medicinal Area has seen a large growth due to the rapid increase in the number of regular 
diseases, as well as the discovery of unknown diseases which are quite lethal. While this is a 
bad news for the global world, it is also a bad news for the medical business as many of them
still dwell on the traditional ways of papers and books. Currently, there are a huge number of
private and public hospitals that store the records manually using physical case studies. It’s a
very formal way of storing data and records but the demerit of storing data in this manner is
that poor handling of record books can lead to loss of data, and there is no provision of
backup.

This can be solved using a computer based system to manage the Business Process.The use of
CAS can enhance the services and the work flow of all the activities that happen in the clinics;
where it helps in reducing the workload of medical staff, the number of man power needed
and making it more manageable and easier to control.

The project “clini-C-are” helps in tying up the medical institutions and the patients together.
Prior to this system a lot of processes we handled manually and consumed a lot of time and
resources. Based on the requirements this system has been designed to enable users to have
easy access to the common information about the clinic. Using this, users will now be able to
search for more information on the prescribed drugs. Patients will be able to interact with
their physicians directly via an inbuilt messaging system. The system also has the provision
for physicians and salespersons to order for more drugs directly with the suppliers. Suppliers
in turn can view the orders placed in the system and approve it as well.

“clini-C-are” is a Microsoft .NET based web application and provides a common platform for the 
different users responsible for the smooth operations of the clinic. This system supports six 
different types of users: 
Guests, Admin, Patients, Physicians, Salesperson and Suppliers.

The Guest User can only view the drugs inventory and the physicians associated with the clinic; 
as well as request the admin for their registration along with a particular role of - Patient, 
Physician, or Supplier. Salesperson are directly appointed by the clinic and thus cannot be 
requested as a role.

Each and every registered user can edit their personal details which are different according to their role.
They can also change their password.

The Admin User can view these requests and can accept or deny them. They can register the users 
for specific roles and an e-mail is sent to the registered e-mail address along with a random 
generated numerical password. The admin can access the information of all the users and can block 
them. They can also delete the drugs from the inventory. The drugs are "deleted" in the sense that 
they will not be visible to the patients for ordering but they will be retained in the  database 
for the sole purpose of not having to enter quite the same data repeatedly.

The Patient User can request the physicians for an appointment. The Specialization of the Physician 
must be selected first. With the help of AJAX, physicians associated with that particular specialization 
are loaded. A failure to do this, results in an error. The Appoitments and their status can be viewed. 
Also, the patients can order drugs from the clinic and can view the order and its status. The patient 
can also send a message to the physician but only after their appointment has been accepted. They can 
then view the subject line to the message and further replies in the "View Messages" page.

The Physician User can view the requested appointments and accept or reject them. They can view and 
update the medical history of those patients who currently have their appointment or are done with it. 
They can reply to the messages sent by the patients and also send new messages to the salesperson, mainly 
regarding the availability of drugs in the clinic. They can then view the subject line to the message and 
further replies in the "View Messages" page.
 
The Salesperson User can add new drugs to the drug inventory and can view those drugs while also being 
able to edit the drug information and delete the drug from inventory. They manage the drug orders raised 
by the patient and can deliver or reject them. They can also place new orders to the supplier for stocking 
up the inventory and view the status of the placed orders. They can reply to the messages sent by the physicians 
and also send new messages to the supplier, mainly regarding the availability of drugs in the clinic. They can 
then view the subject line to the message and further replies in the "View Messages" page.

The Supplier User manages the drug orders raised by the salesperson and can deliver or reject them. They can 
reply to the messages sent by the salesperson.

Technologies Used:
- C# .NET
- ASP.NET MVC Framework
- Entity Framework
- HTML, CSS, Bootstrap, AJAX

Database:
- SQL Server

IDE:
- Visual Studio 2019
- Microsoft SQL Server Management Studio

Features:
- All users can view drugs inventory and active physicians
- Guest can request admin for registration 
- Registered Users can login
- All users can edit their personal details
- All users can change their password
- Admin can register and block users
- Admin can delete drugs from drugs inventory
- Patient can request appointment from physician
- Patient can order drugs from drugs inventory
- Patient can send messages to physician
- Physician can accept/reject requested appointments
- Physician can update patients' history
- Physician can send messages to salesperson
- Physician can reply to patients' messages
- Salesperson can add drugs to the drugs inventory
- Salesperson can edit/delete drugs from the drugs inventory
- Salesperson can deliver/reject orders raised by patients
- Salesperson can order drugs from supplier
- Salesperson can send messages to supplier
- Salesperson can reply to physicians' messages
- Supplier can deliver/reject orders raised by salespersons
- Supplier can reply to salespersons' messages
