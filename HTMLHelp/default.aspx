<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="HTMLHelp.Default" EnableEventValidation="false" %>

<!DOCTYPE html>


<html>
<head runat="server">
    <title><asp:Literal ID="lit1" runat="server" Text="HTML Help" /></title>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1">
    <link href="~/style/form.css?v=1.4.1." rel="stylesheet" />
    <script type="text/javascript">
	
    function linkTo(location) 
	{
		var tv=document.getElementById("tvFolders");
		
		var tvItems = tv.getElementsByTagName("a");
		
		var count=tvItems.length;
		var thisID;
		for (var n=0; n<count;n++)
		{
			var ele=tvItems[n];
			if (ele.innerHTML==location)
				thisID=ele.id;
		}
		var thisLink = document.getElementById(thisID).getAttribute('href');
		setTimeout(thisLink, 10);
		
	}

    </script>
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
    <main>
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

  

    function ShowTree() {
        if (TreeVisible) {
            $("container").css("grid-template-columns", "0 auto");
            $("#ContentPlaceHolder1_upAside").hide();
            TreeVisible = false;
        }
        else {
            $("container").css("grid-template-columns", "0.6fr 0.4fr");
            $("#ContentPlaceHolder1_upAside").show();
            TreeVisible = true;
        }
    }

    function ShowScreenSize() {
        var txt = "";
        txt += "<p>Total width/height: " + screen.width + "*" + screen.height + "</p>";
        txt += "<p>Available width/height: " + screen.availWidth + "*" + screen.availHeight + "</p>";
        txt += "<p>Color depth: " + screen.colorDepth + "</p>";
        txt += "<p>Color resolution: " + screen.pixelDepth + "</p>";

        alert(txt);
    }

</script>


</html>
