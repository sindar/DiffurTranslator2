function [ts,data]=t_euler()
x0=[100;0;0];
t0 =0; dt = 0.5; tn = 10;
Nsteps = round(tn/dt)
ts = zeros(Nsteps,1)
data = zeros(Nsteps,length(x0))
ts(1) = t0
data(1,:) = x0'
t1 = 0

for i =1:Nsteps
%  k1=feval(@t_funsys, ts(i), data(i));
%  k2=feval(@t_funsys, ts(i), data(i)+dt*k1);
%  data(i+1)=data(i)+(dt/2)*(k1+k2);

dxdt= feval(@t_funsys,t0,x0)
dxdt1= feval(@t_funsys,t1,x0+dt*dxdt)

x0=x0+(dt/2)*(dxdt+dxdt1);
t0 = t0+dt
ts(i+1) = t0
data(i+1,:) = x0'

%x0=x0+dxdt*dt
%t0 = t0+dt
%ts(i+1) = t0
%data(i+1,:) = x0'
end

f = figure('Visible','off')
plot (ts,data(:,[1,2,3]),'lineWidth',3);
grid on
legend('x`1','x`2','x`3')
print('-dbmp','-r80','graf_heun.bmp')
end

