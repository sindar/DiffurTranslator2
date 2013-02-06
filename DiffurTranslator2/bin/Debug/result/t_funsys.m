function dxdt=t_funsys(t,x);
a=0.001;
b=0.07;
c=0.01;

dxdt=zeros(3, 1);

dxdt(1)=(-a)*x(1)*x(2);
dxdt(2)=a*x(1)*x(2)-(b+c)*x(2);
dxdt(3)=b*x(2);

