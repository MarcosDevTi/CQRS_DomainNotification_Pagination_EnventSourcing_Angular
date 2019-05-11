﻿using System;
using Arch.Cqrs.Client.AutoMapper;
using Arch.Cqrs.Client.Command.Customer;
using AutoMapper;

namespace Arch.Cqrs.Client.Event.Customer
{
    public class CustomerCreated : Infra.Shared.Cqrs.Event.Event, ICustomMapper
    {
        public CustomerCreated() { }
        public CustomerCreated(
            Guid id,
            string firstName,
            string lastName,
            string email,
            DateTime birthDate,
            string street,
            string number,
            string city,
            string zipCode)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;
            Street = street;
            Number = number;
            City = city;
            ZipCode = zipCode;
            AggregateId = id;
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }

        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public void Map(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<CreateCustomer, CustomerCreated>()
                .ConstructUsing(
                    x => new CustomerCreated(
                        x.Id.Value,
                        x.FirstName,
                        x.LastName,
                        x.Email,
                        x.BirthDate,
                        x.Street,
                        x.Number,
                        x.City,
                        x.ZipCode))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
