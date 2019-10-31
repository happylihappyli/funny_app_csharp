
function Pearson_Correlation(data){
    var count=s_ui.datagrid_rows("grid1")-1;
    var data=new Array(count);
    s_ui.msg("step1");
    for (var i=0;i<count;i++){
        data[i]= new Array(2);  
        data[i][0]=parseFloat(s_ui.datagrid_read("grid1",i,0));
        data[i][1]=parseFloat(s_ui.datagrid_read("grid1",i,1));
    }
    s_ui.msg("step2");
    
    var r=Pearson_Correlation_Sub(data);
    s_ui.msg(r);
    s_ui.Web_Content("web1","Pearson相关系数："+r);
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
        //s_ui.msg(data[i][0]);
        xy_sum+= (data[i][0]-a_avg)*(data[i][1]-b_avg);
        xx_sum+= (data[i][0]-a_avg)*(data[i][0]-a_avg);
        yy_sum+= (data[i][1]-b_avg)*(data[i][1]-b_avg);
    }
    //s_ui.msg(xy_sum+","+xx_sum+","+yy_sum);
    var r=xy_sum/(s_math.sqrt(xx_sum)*s_math.sqrt(yy_sum));
    return r;
}


function sortNumber(a,b)
{
    return a - b
}

//计算排序下标
function calculate_index(a){
    var count=a.length;
    //s_ui.msg(count);
    
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
    var count=s_ui.datagrid_rows("grid1")-1;
    //s_ui.msg(count);
    
    var a=new Array(count);
    var b=new Array(count);
    var data=new Array(count);
    for (var i=0;i<count;i++){
        data[i]= new Array(2);
        data[i][0]=parseFloat(s_ui.datagrid_read("grid1",i,0));
        data[i][1]=parseFloat(s_ui.datagrid_read("grid1",i,1));
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
    //s_ui.datagrid_Set("grid1",i,2,);
    
    var index=calculate_index(a);
    
    
    strLine="";
    for(var key in index){
        strLine+=index[key]+"(k="+key+")\n";
    }
    //s_ui.msg("...:"+strLine);
    
    
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
    //s_ui.msg("...:"+strLine);
    
    for (var i=0;i<count;i++){
        var key=data[i][1]+"";
        data2[i][1]=index[key];
    }
    
    //s_ui.msg("step3");
    
    var r=Pearson_Correlation_Sub(data2);

    s_ui.Web_Content("web1","SpearMan相关系数："+r);
    
}


