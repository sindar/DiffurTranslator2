function dxdt=t_funsys(t,x);
k=0.5;
l=0.1;
m=0.25;
n=0.1;

dxdt=zeros(3, 1);

dxdt(1)=x(3)*l + x(2)*n-x(1)*k-x(1)*m;
dxdt(2)=x(1)*m - x(2)*n;
dxdt(3)=x(1)*k - x(3)*l;

