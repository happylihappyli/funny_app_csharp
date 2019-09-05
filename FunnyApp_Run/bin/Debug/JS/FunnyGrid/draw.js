

function draw_line(){
    var mydata="";
    var count=sys.DataGrid_Rows("grid1")-1;
    var data=new Array(count);
    for (var i=0;i<count;i++){
        data[i]= new Array(2);
        data[i][0]=parseFloat(sys.DataGrid_Read("grid1",i,0));
        data[i][1]=parseFloat(sys.DataGrid_Read("grid1",i,1));
        mydata+="["+data[i][0]+","+data[i][1]+"],";
    }
    if (mydata.endsWith(",")){
        mydata=mydata.substring(0,mydata.length-1);
    }
    
    var html=sys.File_Read("@\\data\\echarts.html.template");
    var data=sys.File_Read("@\\data\\tmp.data");
    data=data.replace("{sys.data.0}",mydata);
    html=html.replace("{sys.data.0}",data);
    sys.File_Save("@\\data\\test.html",html);
    sys.Run_App("chrome.exe",sys.Path_JS()+"\\data\\test.html");
    //sys.Web_Content("web1",html);
}
