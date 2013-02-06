function [ts,data]=t_euler()
x0=[620.0;10.1;70.1];
t0 =0; dt = 0.6; tn = 50;
Nsteps = round(tn/dt)
ts = zeros(Nsteps,1)
data = zeros(Nsteps,length(x0))
ts(1) = t0
data(1,:) = x0'
for i =1:Nsteps
dxdt= feval(@t_funsys,t0,x0)
x0=x0+dxdt*dt
t0 = t0+dt
ts(i+1) = t0
data(i+1,:) = x0'
end
f = figure('Visible','off')
plot (ts,data(:,[1,2,3]),'lineWidth',3);
grid on
legend('x`1','x`2','x`3')
print('-dbmp','-r80','graf_eu.bmp')
end

