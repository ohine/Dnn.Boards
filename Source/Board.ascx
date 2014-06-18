<%@ Control language="C#" Inherits="DotNetNuke.Modules.Boards.Board" AutoEventWireup="false"  Codebehind="Board.ascx.cs" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" %>
<dnn:DnnJsInclude FilePath="~/Resources/Shared/Scripts/jquery.history.js" />
<asp:Panel id="divBoard" runat="server" CssCclass="dnnForm dnnClear">
	<div class="myBoard">
		<div id="interactions">
			<div id="message">
				<span data-bind="flash: lastAction"></span>
			</div>
			<div id="error" style="display:none;">
				<span data-bind="flash: lastError"></span>
			</div>
		</div>
		<div id="main" data-bind="foreach: lists">
			<div class="list">
				<h3 data-bind="text: name"></h3>
				<div class="cards" data-bind="sortable: { data: cards }">
					<div class="card">
						<div><span data-bind="text: title"></span></div>
						<div class="dnnRight">
							<a href="#" data-bind="click: $root.openCard"><%= LocalizeString("EditBtn") %></a>
							<a href="#" data-bind="click: $parent.removeCard"><%= LocalizeString("DeleteBtn") %></a>
						</div>
					</div>
				</div>
				<!-- ko if: showAddCard -->
				<a href="#" class="hlCompose" data-bind="click: $root.expandCompose"><%= LocalizeString("AddCard") %></a>
				<div style="display:none;" id="composeArea">
					<textarea class="cardTitle" id="textTitle"></textarea>
					<div class="dnnClear subActions">
						<a href="#" class="dnnPrimaryAction" data-bind="click: createCard"><%= LocalizeString("AddBtn") %></a>
						<a href="#" class="dnnSecondaryAction" data-bind="click: $root.collapseCompose"><%= LocalizeString("CloseBtn")%></a>
					</div>
				</div>
				<!-- /ko -->
			</div>
		</div>
	</div>
	<div id="divModal" style="display:none;">
		<!-- ko with: selectedCard -->
		<h4 data-bind="text: title, click: editTitle, visible: view() == 'read' || view() == 'description'" id="headTitle"></h4>
		<input type="text" id="iTitle" data-bind="value: title, visible: view() == 'title'">
		<div class="dnnClear subActions" id="titleActions" data-bind="visible: view() == 'title'">
			<a href="#" class="dnnPrimaryAction" data-bind="click: saveTitle"><%= LocalizeString("SaveBtn")%></a>
			<a href="#" class="dnnSecondaryAction" data-bind="click: cancelEditTitle"><%= LocalizeString("CloseBtn")%></a>
		</div>
		<div class="dnnLeft cardLeftColumn">
			<div style="display:none;">
				<span>Due Date here</span>
			</div>
			<div>
				<p data-bind="text: content, visible: view() == 'read' || view() == 'title'" id="pDescription"></p>
				<textarea data-bind="value: content, visible: view() == 'description'" id="textDescription"></textarea>
				<div class="dnnClear subActions" id="descriptionActions" data-bind="visible: view() == 'description'">
					<a href="#" class="dnnPrimaryAction" data-bind="click: saveDescription"><%= LocalizeString("SaveBtn") %></a>
					<a href="#" class="dnnSecondaryAction" data-bind="click: cancelEditDescription"><%= LocalizeString("CloseBtn") %></a>
				</div>
				<a href="#" id="hlDescription" data-bind="click: editDescription, visible: view() == 'read' || view() == 'title'"><%= LocalizeString("EditDescription") %></a>
			</div>
		</div>
		<div class="dnnLeft cardRightColumn">
			<h5><%= LocalizeString("Members") %></h5>
			<div class="innerArea"><img src='<%= ResolveUrl("~/images/no_avatar.gif") %>' width="40" height="40" /></div>
			<h5><%= LocalizeString("Actions") %></h5>
			<div class="innerArea buttonGroup">
				<a id="btnDueDate" class="dnnTertiaryAction"><%= LocalizeString("DueDate") %></a>
				<a id="btnArchive" class="dnnTertiaryAction"><%= LocalizeString("Archive") %></a>
			</div>
		</div>
		<!-- /ko -->
	</div>
</asp:Panel>
<script language="javascript" type="text/javascript">
    jQuery(document).ready(function ($) {
        var moduleId = parseInt('<%= ModuleContext.ModuleId %>', 10);
        
        var bm = new dnn.boards.Board($, ko, {
			boardId: '<% = BoardId %>',
		    moduleId: moduleId,
			moduleScope: $('#divBoard')[0]
		});
		
		bm.init();
	});
</script>