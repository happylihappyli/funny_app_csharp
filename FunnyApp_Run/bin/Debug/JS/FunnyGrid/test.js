
function chi_square(data){
    
    var x=chi_square_sub(data);
    
    sys.Web_Content("web1","卡方检验："+x);
}

function chi_square_sub(data){
    
    var count=sys.DataGrid_Rows("grid1")-1;
    //sys.Msg(count);
    
    var a=new Array(count);
    var b=new Array(count);
    var data=new Array(count);
    var x=0;
    for (var i=0;i<count;i++){
        data[i]= new Array(2);
        data[i][0]=parseFloat(sys.DataGrid_Read("grid1",i,0));
        data[i][1]=parseFloat(sys.DataGrid_Read("grid1",i,1));
        a[i]=data[i][0];
        b[i]=data[i][1];
        var delta=a[i]-b[i];
        //sys.Msg(delta);
        x+=delta*delta/b[i];
    }
    return x;
}

function WOE_IV(data){
    
    var count=sys.DataGrid_Rows("grid1")-1;
    //sys.Msg(count);
    
    var a=new Array(count);
    var b=new Array(count);
    var data=new Array(count);
    
    var sum_a=0;
    var sum_b=0;
    for (var i=0;i<count;i++){
        data[i]= new Array(2);
        data[i][0]=parseFloat(sys.DataGrid_Read("grid1",i,0));
        data[i][1]=parseFloat(sys.DataGrid_Read("grid1",i,1));
        a[i]=data[i][0];
        b[i]=data[i][1];
        
        sum_a+=a[i];
        sum_b+=b[i];
    }
    
    var IV=0;
    for (var i=0;i<count;i++){
        var x=(a[i]/sum_a)/(b[i]/sum_b);
        var woe=sys.Math_ln(x);
        sys.DataGrid_Set("grid1",i,2,woe);
        
        IV+=((a[i]/sum_a)-(b[i]/sum_b))*woe;
    }
    var strInfo="<br> &lt; 0.03 无预测能力<br>"
            +"0.03 - 0.09 低<br>"
            +"0.1 - 0.29 中<br>"
            +"0.3 - 0.49 高<br>"
            +"&gt;=0.5 极高<br>";

    sys.Web_Content("web1","IV="+IV+strInfo);
}


