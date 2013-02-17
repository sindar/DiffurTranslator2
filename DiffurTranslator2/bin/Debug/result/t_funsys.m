function dxdt=t_funsys(t,x);
a=0.9;
b=0.3;
c=0.45;
d=0.3;

dxdt=zeros(3, 1);

dxdt(1)=x(3)*b + x(2)*d-x(1)*a-x(1)*c;
dxdt(2)=x(1)*c - x(2)*d;
dxdt(3)=x(1)*a - x(3)*b;

