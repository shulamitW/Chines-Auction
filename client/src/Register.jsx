import React, { useEffect, useState } from 'react';
import { useForm, Controller } from 'react-hook-form';
import { InputText } from 'primereact/inputtext';
import { Button } from 'primereact/button';
import { Dropdown } from 'primereact/dropdown';
//import { Calendar } from 'primereact/calendar';
import { Password } from 'primereact/password';
import { Checkbox } from 'primereact/checkbox';
import { Dialog } from 'primereact/dialog';
import { Divider } from 'primereact/divider';
import { classNames } from 'primereact/utils';
import axios from 'axios';
import './FormDemo.css';
import { useNavigate } from 'react-router-dom'

axios.defaults.baseURL="http://localhost:5018"
// import { CountryService } from '../service/CountryService';

export default function Register () {
    //const [countries, setCountries] = useState([]);
    const [showMessage, setShowMessage] = useState(false);
    const [formData, setFormData] = useState({});
    const navigate = useNavigate();

    //const countryservice = new CountryService();
    const defaultValues = {
        FirstName: '',
        LastName: '',
        password: '',
        UserName: '',
        Email: '',
        Phone: '',
        Address: ''
    }


    // useEffect(() => {
    //     countryservice.getCountries().then(data => setCountries(data));
    // }, []); // eslint-disable-line react-hooks/exhaustive-deps

    const { control, formState: { errors }, handleSubmit, reset } = useForm({ defaultValues });

    const onSubmit = (data) => {
        setFormData(data);

        setShowMessage(true);
        addUser(data);
        reset();
    };

    const getFormErrorMessage = (name) => {
        return errors[name] && <small className="p-error">{errors[name].message}</small>
    };

    const dialogFooter = <div className="flex justify-content-center"><Button label="OK" className="p-button-text" autoFocus onClick={() => {setShowMessage(false); navigate(`../Presents/`)}} /></div>;
    const passwordHeader = <h6>Pick a password</h6>;
    const passwordFooter = (
        <React.Fragment>
            <Divider />
            <p className="mt-2">Suggestions</p>
            <ul className="pl-2 ml-2 mt-0" style={{ lineHeight: '1.5' }}>
                <li>At least one lowercase</li>
                <li>At least one uppercase</li>
                <li>At least one numeric</li>
                <li>Minimum 8 characters</li>
            </ul>
        </React.Fragment>
    );


    const addUser=(data)=>{
        const user={
        FirstName: data.FirstName,
        LastName: data.LastName,
        UserName: data.UserName,      
        password: data.password,
        Email: data.Email,
        Phone: data.Phone,
        Address: data.Address
        }
       alert(user.FirstName)
        try{
            axios.post(`/User/Register`,user)
            .then(res=>console.log(res.data.UserName))
            
            // navigate(`../Calander/${newUser.userId}`)
          }   
          catch(err){
            alert(err)
          }
    }
    return (
        <div className="form-demo">
            <Dialog visible={showMessage} onHide={() => setShowMessage(false)} position="top" footer={dialogFooter} showHeader={false} breakpoints={{ '960px': '80vw' }} style={{ width: '30vw' }}>
                <div className="flex justify-content-center flex-column pt-6 px-3">
                    <i className="pi pi-check-circle" style={{ fontSize: '5rem', color: 'var(--green-500)' }}></i>
                    <h5>Registration Successful!</h5>
                    <p style={{ lineHeight: 1.5, textIndent: '1rem' }}>
                        Your account is registered under name <b>{formData.name}</b> ; it'll be valid next 30 days without activation. Please check <b>{formData.email}</b> for activation instructions.
                    </p>

                </div>
            </Dialog>

            <div className="flex justify-content-center">
                <div className="card">
                    <h2 className="text-center">Register</h2>
                    <form onSubmit={handleSubmit(onSubmit)} className="p-fluid">
                        <div className="field">
                            <span className="p-float-label">
                                <Controller name="FirstName" control={control} rules={{ required: 'First Name is required.' }} render={({ field, fieldState }) => (
                                    <InputText id={field.name} {...field} autoFocus className={classNames({ 'p-invalid': fieldState.invalid })} />
                                )} />
                                <label htmlFor="FirstName" className={classNames({ 'p-error': errors.name })}>First Name*</label>
                            </span>
                            {getFormErrorMessage('FirstName')}
                        </div>
                            
                        <div className="field">
                            <span className="p-float-label">
                                <Controller name="LastName" control={control} rules={{ required: 'LastName is required.' }} render={({ field, fieldState }) => (
                                    <InputText id={field.name} {...field} autoFocus className={classNames({ 'p-invalid': fieldState.invalid })} />
                                )} />
                                <label htmlFor="LastName" className={classNames({ 'p-error': errors.name })}>Last Name*</label>
                            </span>
                        {getFormErrorMessage('LastName')}
                        </div>
                        
                        <div className="field">
                            <span className="p-float-label">
                                <Controller name="UserName" control={control} rules={{ required: 'User Name is required.' }} render={({ field, fieldState }) => (
                                    <InputText id={field.name} {...field} autoFocus className={classNames({ 'p-invalid': fieldState.invalid })} />
                                )} />
                                <label htmlFor="UserName" className={classNames({ 'p-error': errors.name })}>User Name*</label>
                            </span>
                            {getFormErrorMessage('UserName')}
                        </div>

                        <div className="field">
                            <span className="p-float-label">
                                <Controller name="password" control={control} rules={{ required: 'Password is required.' }} render={({ field, fieldState }) => (
                                    <Password id={field.name} {...field} toggleMask className={classNames({ 'p-invalid': fieldState.invalid })} header={passwordHeader} footer={passwordFooter} />
                                )} />
                                <label htmlFor="Password" className={classNames({ 'p-error': errors.name })}>Password*</label>
                            </span>
                            {getFormErrorMessage('password')}
                        </div>

                        <div className="field">
                            <span className="p-float-label p-input-icon-right">
                            <i className="pi pi-envelope" />
                                <Controller name="Email" control={control}
                                    rules={{ required: 'Email is required.', pattern: { value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i, message: 'Invalid email address. E.g. example@email.com' }}}
                                    render={({ field, fieldState }) => (
                                        <InputText id={field.name} {...field} className={classNames({ 'p-invalid': fieldState.invalid })} />
                                )} />
                               
                                <label htmlFor="Email" className={classNames({ 'p-error': !!errors.name })}>Email*</label>
                            </span>
                            {getFormErrorMessage('Email')}
                        </div>

                        <div className="field">
                            <span className="p-float-label">
                                <Controller name="Phone" control={control} rules={{ required: 'Phone is required.' }} render={({ field, fieldState }) => (
                                    <InputText id={field.name} {...field} autoFocus className={classNames({ 'p-invalid': fieldState.invalid })} />
                                )} />
                                <label htmlFor="Phone" className={classNames({ 'p-error': errors.name })}>Phone*</label>
                            </span>
                        {getFormErrorMessage('Phone')}
                        </div>

                        <div className="field">
                            <span className="p-float-label">
                                <Controller name="Address" control={control} rules={{ required: 'Address is required.' }} render={({ field, fieldState }) => (
                                    <InputText id={field.name} {...field} autoFocus className={classNames({ 'p-invalid': fieldState.invalid })} />
                                )} />
                                <label htmlFor="Address" className={classNames({ 'p-error': errors.name })}>Address*</label>
                            </span>
                        {getFormErrorMessage('Address')}
                        </div>

                        <div className="field-checkbox">
                            <Controller name="accept" control={control} rules={{ required: true }} render={({ field, fieldState }) => (
                                <Checkbox inputId={field.name} onChange={(e) => field.onChange(e.checked)} checked={field.value} className={classNames({ 'p-invalid': fieldState.invalid })} />
                            )} />
                            <label htmlFor="accept" className={classNames({ 'p-error': errors.accept })}>I agree to the terms and conditions*</label>
                        </div>

                        <Button type="submit" label="Submit" className="mt-2" />
                    </form>
                </div>
            </div>
        </div>
    );
}