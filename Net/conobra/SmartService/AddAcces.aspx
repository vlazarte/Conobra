<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true"
    CodeBehind="AddAcces.aspx.cs" Inherits="SmartService.AddAcces" %>

<asp:Content ContentPlaceHolderID="contenido" runat="server">
    <div class="">
        <div class="page-title">
            <div class="title_left">
                <h3>
                    Nuevo Access</h3>
            </div>
        </div>
        <div class="clearfix">
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>
                            Datos de Access</h2>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="x_content">
                        <br />
                        <form method="POST" runat="server" id="container" data-parsley-validate class="form-horizontal form-label-left">
                        <div class="form-group">
                            <label class="control-label col-md-3 col-sm-3 col-xs-12" for="first-name">
                                Token<span class="required">*</span>
                            </label>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <input type="text" id="first-name" required="required" class="form-control col-md-7 col-xs-12"
                                    name="token" value="">
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-3 col-sm-3 col-xs-12" for="first-name">
                                Realm<span class="required">*</span>
                            </label>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <input type="text" id="Text1" required="required" class="form-control col-md-7 col-xs-12"
                                    name="Realm" value="">
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-3 col-sm-3 col-xs-12">
                                Tipo Access</label>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <select class="select2_single form-control" name="type" tabindex="-1">
                                    <option></option>
                                    <option value="add">Agregar</option>
                                    <option value="edit">Editar</option>
                                    <option value="import">Import</option>
                                    <option value="fetchrow">FetchRow</option>
                                    <option value="delet">Delete</option>
                                    <option value="doquery">Doquery</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-3 col-sm-3 col-xs-12" for="first-name">
                                Dbid<span class="required">*</span>
                            </label>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <input type="text" id="Text2" required="required" class="form-control col-md-7 col-xs-12"
                                    name="dbid" value="">
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-3 col-sm-3 col-xs-12" for="first-name">
                                query<span class="required">*</span>
                            </label>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <input type="text" id="Text3" required="required" class="form-control col-md-7 col-xs-12"
                                    name="query" value="">
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-3 col-sm-3 col-xs-12" for="first-name">
                                Qid<span class="required">*</span>
                            </label>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <input type="text" id="Text4" required="required" class="form-control col-md-7 col-xs-12"
                                    name="qid" value="">
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-md-3 col-sm-3 col-xs-12" for="first-name">
                                Clist<span class="required">*</span>
                            </label>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <input type="text" id="Text5" required="required" class="form-control col-md-7 col-xs-12"
                                    name="clist" value="">
                            </div>
                        </div>
                        <div class="access">
                            <div class="form-group">
                                <div class="form-group col-md-3 text-right">
                                    <label class="control-label" for="first-name">
                                        Parametros<span class="required">*</span>
                                    </label>
                                </div>
                                <div class="form-group col-md-3">
                                    <input type="text" value="" name="name1" placeholder="Name" class="form-control"
                                        id="inputCity">
                                </div>
                                <div class="form-group col-md-3">
                                    <input type="text" value="" name="fid1" placeholder="Fid" class="form-control">
                                </div>
                                <input class="form-check-input" type="checkbox" name="require1" value="true">
                                <label class="form-check-label" for="gridCheck">
                                    Requerido
                                </label>
                            </div>
                        </div>
                        <div class="col text-right" style="width: 285px">
                            <input class="btn btn-warning btn-sm" style="" onclick="add();" type="reset" value="Añadir Parametro">
                        </div>
                        <div class="col text-right" style="width: 250px">
                        </div>
                        <div class="col text-right" style="width: 285px">
                            <input type="hidden" name="cantidad" value="1" id="cantidad" />
                            <br />
                            <button type="submit" class="btn btn-success">
                                Agregar</button>
                        </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var id = 1;
        function add() {
            id++;
            var html = "";
            html += "<div class='row'>";
            html += "<div class='col col-md-3 text-right'>";
            html += "<label class='control-label' for='first-name'>Parametros<span class='required'>*</span></label>";
            html += "</div>";
            html += "<div class='col col-md-3'>";
            html += "<input type='text' value='' name='name" + id + "' placeholder='Name' class='fname form-control'>";
            html += "</div>";
            html += "<div class='form-group col-md-3'>";
            html += "<input type='text' value='' name='fid" + id + "' placeholder='Fid' class='form-control'>";
            html += "</div>";
            html += "<input class='form-check-input' type='checkbox' name='require" + id + "' value='true'>";
            html += "<label class='form-check-label' for='gridCheck'>Requerido</label>";
            html += " <a onclick='if(confirm(\"estas seguro de eliminar\")) Eliminar(this);'><img style='height: 24px;' src='images/delete.png' /></a>";
            html += "</div>";
            html += "";

            $("#container .access").append(html);
            $("#cantidad").val(id);

        }
        function Eliminar(element) {
            var padre = $(element).parent();
            padre.find('input.fname').val("");
            padre.hide();
            alert("Eliminado Correctamente");


        }

    </script>
</asp:Content>
