<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true"
    CodeBehind="ListAccess.aspx.cs" Inherits="SmartService.ListAccess" %>

<asp:Content ContentPlaceHolderID="contenido" runat="server">
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="x_panel">
                <div class="x_title">
                    <h2>
                        Lista de Access
                    </h2>
                    <ul class="nav navbar-right panel_toolbox">
                        <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a></li>
                        <li class="dropdown"><a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button"
                            aria-expanded="false"><i class="fa fa-wrench"></i></a>
                            <ul class="dropdown-menu" role="menu">
                                <li><a href="#">Settings 1</a> </li>
                                <li><a href="#">Settings 2</a> </li>
                            </ul>
                        </li>
                        <li><a class="close-link"><i class="fa fa-close"></i></a></li>
                    </ul>
                    <div class="clearfix">
                    </div>
                </div>
                <div class="x_content">
                    <table class="table" id="container">
                        <tr>
                            <th width='110'>
                            </th>
                            <th>
                                id
                            </th>
                            <th>
                                Token
                            </th>
                            <th>
                                Realm
                            </th>
                            <th>
                                Type
                            </th>
                            <th>
                                Dbid
                            </th>
                            <th>
                                query
                            </th>
                            <th>
                                Qid
                            </th>
                            <th>
                                Clist
                            </th>
                            <th style="width: 250px">
                                Param
                            </th>
                        </tr>
                        <% for (int i = 0; i < root.list.Count; i++)
                           { %>
                        <tr class="data">
                            <td method="POST">
                                <a href="EdditAccess.aspx?id=<%Response.Write(root.list[i].id); %>"><img style="height: 24px;" src="images/edit.png" /></a> 
                                    &nbsp; 
                                <a href="EliminarAccess.aspx?id=<%Response.Write(root.list[i].id); %>"><img style="height: 24px;" src="images/delete.png" /></a>
                            </td>
                            <td class="inner">
                                <% Response.Write(root.list[i].id); %>
                            </td>
                            <td class="inner">
                                <% Response.Write(root.list[i].token); %>
                            </td>
                            <td class="inner">
                                <% Response.Write(root.list[i].realm); %>
                            </td>
                            <td class="inner">
                                <% Response.Write(root.list[i].type); %>
                            </td>
                            <td class="inner">
                                <% Response.Write(root.list[i].dbid); %>
                            </td>
                            <td class="inner">
                                <% Response.Write(root.list[i].query); %>
                            </td>
                            <td class="inner">
                                <% Response.Write(root.list[i].qid); %>
                            </td>
                            <td class="inner">
                                <% Response.Write(root.list[i].clist); %>
                            </td>
                            <td class="inner">
                                <%for (int j = 0; j < root.list[i].paramethers.Count; j++)
                                  { %>
                                Nombre:
                                <label>
                                    <%Response.Write(root.list[i].paramethers[j].name); %></label>
                                <%if (root.list[i].paramethers[j].fid != -1)
                                  {%>
                                Fid:
                                <label>
                                    <%Response.Write(root.list[i].paramethers[j].fid); %></label>
                                <%} %>
                                Require
                                <label>
                                    <%Response.Write(root.list[i].paramethers[j].require); %></label>
                                <% } %>
                            </td>
                        </tr>
                        <%
                            }
                        %>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
