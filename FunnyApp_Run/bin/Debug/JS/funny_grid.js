String.prototype.startsWith=function(str){     
  var reg=new RegExp("^"+str);     
  return reg.test(this);        
}  

String.prototype.endsWith=function(str){     
  var reg=new RegExp(str+"$");     
  return reg.test(this);        
}


var userName="none";
var log_msg="";

var msg_id=0;
var myMap=[];

//消息和发送计数器
function C_Msg(ID,Msg){
    this.ID=ID;
    this.Msg=Msg;
    this.Count=0;
}


function text_keydown(data){
    if (data==13){
        send_msg_click();
    }
}



function Node_Click(data){
    
}

function modify_click(data){
    var index=parseInt(data);
    var a=sys.DataGrid_Read("grid1",index,0);
    
    sys.Msg("test:"+a);
}

function init_data(data){
    sys.ListBox_Clear("list_friend");
    sys.ListBox_Add("list_friend","*");
    var strLine="{\"from\":\""+userName+"\",\"type\":\"list.all\",\"to\":\"\",\"message\":\"\"}";
    sys.Send_Msg("sys_event",strLine);
}

function add_data(data){
    sys.DataGrid_Init_Column("grid1",6);
    sys.DataGrid_Add_Line("grid1","0,0",",");
    sys.DataGrid_Add_Line("grid1","1,2",",");
    sys.DataGrid_Add_Line("grid1","2,3",",");
    sys.DataGrid_Add_Line("grid1","3,3.5",",");
    sys.DataGrid_Add_Line("grid1","4,4",",");
    sys.DataGrid_Add_Line("grid1","5,4.5",",");
    sys.DataGrid_Add_Line("grid1","6,5",",");
    sys.DataGrid_Add_Line("grid1","7,6",",");

    sys.DataGrid_Add_Button("grid1","modify","修改","modify_click");
}

function clear_data(data){
    sys.DataGrid_Clear("grid1");
}

function tree_init(data){

    sys.Tree_Add_Node_Root("tree1","root","我的电脑","Node_Click");
    sys.Tree_Add_Node("tree1","root","test","test","Node_Click");
    sys.Tree_Add_Node_Root("tree1","C","C:","Node_Click");
    sys.Tree_Add_Node_Root("tree1","D","D:","Node_Click");
    sys.Tree_Add_Node_Root("tree1","E","E:","Node_Click");
}


function Pearson_Correlation(data){
    var count=sys.DataGrid_Rows("grid1")-1;
    var data=new Array(count);
    for (var i=0;i<count;i++){
        data[i]= new Array(2);  
        data[i][0]=parseFloat(sys.DataGrid_Read("grid1",i,0));
        data[i][1]=parseFloat(sys.DataGrid_Read("grid1",i,1));
    }
    
    var r=Pearson_Correlation_Sub(data);

    sys.Web_Content("web1","Pearson相关系数："+r);
}

function Pearson_Correlation_Sub(data){
    var a_sum=0;
    var b_sum=0;
    var count=data.length;
    for (var i=0;i<count;i++){
        a_sum += data[i][0];
        b_sum += data[i][1];
    }
    
    var a_avg=a_sum/count;
    var b_avg=b_sum/count;
    
    var xy_sum=0;
    var xx_sum=0;
    var yy_sum=0;
    for (var i=0;i<count;i++){
        //sys.Msg(data[i][0]);
        xy_sum+= (data[i][0]-a_avg)*(data[i][1]-b_avg);
        xx_sum+= (data[i][0]-a_avg)*(data[i][0]-a_avg);
        yy_sum+= (data[i][1]-b_avg)*(data[i][1]-b_avg);
    }
    //sys.Msg(xy_sum+","+xx_sum+","+yy_sum);
    var r=xy_sum/(sys.Math_sqrt(xx_sum)*sys.Math_sqrt(yy_sum));
    return r;
}


function sortNumber(a,b)
{
    return a - b
}

//计算排序下标
function calculate_index(a){
    var count=a.length;
    //sys.Msg(count);
    
    var index=[];
    var iNext;
    for (var i=0;i<count;){
        var count_same=0;//计算相同的有多少个
        for (var j=i+1;j<count;j++){
            if (a[j]==a[i]){
                count_same+=1;
                iNext=j+1;
            }else{
                iNext=j;
                break;
            }
        }
        var k=i+count_same/2;
        index[a[i]+""]=k;
        if (i==count-1) break;
        i=iNext;
    }
    return index;
}

function Spearman_Correlation(data){
    var count=sys.DataGrid_Rows("grid1")-1;
    //sys.Msg(count);
    
    var a=new Array(count);
    var b=new Array(count);
    var data=new Array(count);
    for (var i=0;i<count;i++){
        data[i]= new Array(2);
        data[i][0]=parseFloat(sys.DataGrid_Read("grid1",i,0));
        data[i][1]=parseFloat(sys.DataGrid_Read("grid1",i,1));
        a[i]=data[i][0];
        b[i]=data[i][1];
    }
    
    var strLine="";
    for (var i=0;i<count;i++){
        strLine+=a[i]+",";
    }
    
    a.sort(sortNumber);
    strLine="";
    for (var i=0;i<count;i++){
        strLine+=a[i]+",";
    }
    //sys.DataGrid_Set("grid1",i,2,);
    
    var index=calculate_index(a);
    
    
    strLine="";
    for(var key in index){
        strLine+=index[key]+"(k="+key+")\n";
    }
    //sys.Msg("...:"+strLine);
    
    
    var data2=new Array(count);
    for (var i=0;i<count;i++){
        data2[i]= new Array(2);
    }
    for (var i=0;i<count;i++){
        data2[i][0]=index[data[i][0]+""];
    }
    
    
    index=calculate_index(b);
    
    strLine="";
    for(var key in index){
        strLine+=index[key]+"(k="+key+")\n";
    }
    //sys.Msg("...:"+strLine);
    
    for (var i=0;i<count;i++){
        var key=data[i][1]+"";
        //sys.Msg("key=:"+key);
        //sys.Msg("index=:"+index[key]);
        data2[i][1]=index[key];
        //sys.Msg("step:"+data2[i][1]);
    }
    
    //sys.Msg("step3");
    
    var r=Pearson_Correlation_Sub(data2);

    sys.Web_Content("web1","SpearMan相关系数："+r);
    
}

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
}



sys.DataGrid_Init("grid1",10,60,750,380);


sys.Web_Init("web1",10,450,750,250);


sys.Menu_Init("Menu1",0,0,800,25);
sys.Menu_Add("Menu1","File","文件(&F)");
sys.Menu_Item_Add("Menu1","File","Test","初始化数据","add_data","");
sys.Menu_Item_Add("Menu1","File","Clear","清空数据","clear_data","");
sys.Menu_Add("Menu1","Data","数据(&D)");
sys.Menu_Item_Add("Menu1","Data","Correlation","相关性","","");
sys.Menu_Item2_Add("Menu1","Data","Correlation","Correlation1","Person","Pearson_Correlation","");
sys.Menu_Item2_Add("Menu1","Data","Correlation","Correlation2","Spearman","Spearman_Correlation","");
sys.Menu_Item_Add("Menu1","Data","Draw","绘图","","");
sys.Menu_Item2_Add("Menu1","Data","Draw","line","绘图","draw_line","");



sys.Show_Form(800,800);
sys.Form_Title("Funny_Grid");

add_data("");

