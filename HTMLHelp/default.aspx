<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="HTMLHelp.Default" EnableEventValidation="false" %>

<!DOCTYPE html>


<html>
<head runat="server">
    <title><asp:Literal ID="lit1" runat="server" Text="HTML Help" /></title>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1">
    <link href="~/style/form.css?v=1.4.1." rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="smMaster" runat="server"  />
    <container>

         <header>
            <div id="title"><asp:Image ID="imgMasterHeader" runat="server" ImageAlign="AbsBottom" ImageUrl="~/images/icons/folderlogo75.png" />
                <div id="MenuButton" onclick="ShowTree()" ></div>
                <asp:Label ID="lblMasterHeader" runat="server" Text="Help" CssClass="HeaderTitle"  />
            </div>
          </header>

        <aside>
            <asp:UpdatePanel ID="upAside" runat="server">
                <ContentTemplate>
                    <asp:TreeView ID="tvFolders" runat="server"
                        OnSelectedNodeChanged="tvFolders_SelectedNodeChanged"
                           SelectedNodeStyle-CssClass="TreeSelected"
                    />
                </ContentTemplate>
            </asp:UpdatePanel>
        </aside>

        <main id="main">
            <asp:UpdatePanel ID="upContent" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Literal ID="page_HTML" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </main>

    </container>
        
    </form>
</body>
<script type="text/javascript">
    var TreeVisible = false;

    // Force to scrolltop of newly loaded content 
    Sys.Application.add_load(function () {
        document.getElementById("main").scrollTo(0, 0);
    });

    // Display a named page
    function linkTo(location) {
        var location = location.replace(/_/g, " ");
        var tv = document.getElementById("tvFolders");

        var tvItems = tv.getElementsByTagName("a");

        var count = tvItems.length;
        var thisID = "";
        for (var n = 0; n < count; n++) {
            var ele = tvItems[n];
            if (ele.innerHTML == location)
                thisID = ele.id;
        }
        if (thisID != "") {
            var thisLink = document.getElementById(thisID);
            if ((thisLink != null) && (thisLink.getAttribute("href").length > 1))
                setTimeout(thisLink.getAttribute('href'), 10);
        }
    }

    // Hide/Show tree in mobile view
    function ShowTree() {
        var container = document.getElementsByTagName("container");
        if ((container != null) && (container.length > 0)) 
        {
            if (TreeVisible) 
            {
                container[0].classList.add("full");
                TreeVisible = false;
            }
            else
            {
                container[0].classList.remove("full");
                TreeVisible = true;
            }
        }
    }

</script>


</html>
