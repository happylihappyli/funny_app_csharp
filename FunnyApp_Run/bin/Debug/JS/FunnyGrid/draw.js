

function draw_line(){
    var mydata="";
    var count=s_ui.DataGrid_Rows("grid1")-1;
    var data=new Array(count);
    for (var i=0;i<count;i++){
        data[i]= new Array(2);
        data[i][0]=parseFloat(s_ui.DataGrid_Read("grid1",i,0));
        data[i][1]=parseFloat(s_ui.DataGrid_Read("grid1",i,1));
        mydata+="["+data[i][0]+","+data[i][1]+"],";
    }
    if (mydata.endsWith(",")){
        mydata=mydata.substring(0,mydata.length-1);
    }
    
    var html=s_file.File_Read(sys.Path_JS()+"\\data\\echarts.html.template");
    var data=s_file.File_Read(sys.Path_JS()+"\\data\\tmp.data");
    data=data.replace("{sys.data.0}",mydata);
    html=html.replace("{sys.data.0}",data);
    s_file.File_Save(sys.Path_JS()+"\\data\\test.html",html);
    s_ui.Run_App("chrome.exe",sys.Path_JS()+"\\data\\test.html");
    //s_ui.Web_Content("web1",html);
}
