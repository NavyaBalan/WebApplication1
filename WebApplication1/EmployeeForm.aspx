<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeForm.aspx.cs" Inherits="YourNamespace.EmployeeForm" %>

<!DOCTYPE html>
<html>
<head>
    <title>Employee and Dependents</title>
    <script type="text/javascript">
        function AddDependentRow() {
            var table = document.getElementById("dependentsTable");
            var rowCount = table.rows.length;
            var row = table.insertRow(rowCount);
            row.innerHTML = '<td><input type="text" name="DependentName" required></td>' +
                            '<td><input type="text" name="Relationship" required></td>' +
                            '<td><input type="number" name="Age" required></td>' +
                            '<td><button type="button" onclick="RemoveDependentRow(this)">Remove</button></td>';
        }

        function RemoveDependentRow(button) {
            var row = button.parentNode.parentNode;
            row.parentNode.removeChild(row);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Employee Details</h2>
        <label>Name:</label> <asp:TextBox ID="txtEmployeeName" runat="server" Required="true" Width="222px"></asp:TextBox><br/>
        <label>Email:</label> <asp:TextBox ID="txtEmail" runat="server" Required="true"></asp:TextBox><br/>
        <label>Phone:</label> <asp:TextBox ID="txtPhone" runat="server" Required="true"></asp:TextBox><br/>
        <label>Department:</label> <asp:TextBox ID="txtDepartment" runat="server" Required="true"></asp:TextBox><br/>

        <h3>Dependents</h3>
        <table id="dependentsTable" border="1">
            <tr>
                <th>Name</th>
                <th>Relationship</th>
                <th>Age</th>
                <th>Action</th>
            </tr>
        </table>
        <button type="button" onclick="AddDependentRow()">Add Dependent</button>
        
        <br/><br/>
        <p>
        <asp:Button ID="btnSave" runat="server" Text="Save Employee" OnClick="btnSave_Click" />
        </p>
    </form>
</body>
</html>
