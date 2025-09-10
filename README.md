# Events Coordinator

A modern Next.js 15+ application with HeroUI components for seamless event management.

![Events Coordinator App](https://github.com/user-attachments/assets/856288f3-4a33-4364-ae6a-b71bfb2d3c7e)

## Features

- ⚡ **Next.js 15+** - Built with the latest Next.js features including App Router, Server Components, and more
- 🎨 **HeroUI Components** - Modern, accessible, and customizable React components built on top of Tailwind CSS
- 🔷 **TypeScript** - Full TypeScript support for better development experience and fewer runtime errors
- 🎯 **Tailwind CSS** - Utility-first CSS framework for rapid UI development
- 📱 **Responsive Design** - Mobile-first responsive design with modern UI patterns

## Tech Stack

- **Framework:** Next.js 15.5.2
- **Runtime:** React 19.1.0
- **UI Library:** HeroUI (formerly NextUI) 2.8.4
- **Styling:** Tailwind CSS v4 with @tailwindcss/postcss
- **Language:** TypeScript 5
- **Animation:** Framer Motion 12.23.12

## Getting Started

### Prerequisites

- Node.js 18+ 
- npm or yarn

### Installation

1. Clone the repository:
```bash
git clone https://github.com/tommol/events-coordinator.git
cd events-coordinator
```

2. Install dependencies:
```bash
npm install
```

3. Run the development server:
```bash
npm run dev
```

4. Open [http://localhost:3000](http://localhost:3000) in your browser.

## Available Scripts

- `npm run dev` - Start the development server
- `npm run build` - Build the application for production
- `npm run start` - Start the production server
- `npm run lint` - Run ESLint for code quality checks
- `npm test` - Run the test suite
- `npm run test:watch` - Run tests in watch mode
- `npm run test:coverage` - Run tests with coverage report

## Project Structure

```
src/
├── app/
│   ├── globals.css      # Global styles and Tailwind imports
│   ├── layout.tsx       # Root layout with HeroUI provider
│   └── page.tsx         # Home page with HeroUI components demo
├── __tests__/           # Test files
│   ├── page.test.tsx    # Home page component tests
│   ├── layout.test.tsx  # Layout component tests
│   └── integration.test.tsx # Integration tests
├── components/          # Reusable React components (to be added)
└── lib/                # Utility functions and configurations (to be added)
```

## HeroUI Components

This application showcases various HeroUI components including:

- **Navigation** - Navbar with branding and user avatar
- **Cards** - Feature cards with headers and descriptions
- **Buttons** - Various button styles (Primary, Secondary, Success, Warning, Danger)
- **Typography** - Headings and text components
- **Layout** - Grid and container components

## Testing

This project includes a comprehensive test suite using Jest and React Testing Library.

### Test Structure

- **Unit Tests** - Test individual components and functions
- **Integration Tests** - Test component interactions and layout structure
- **Accessibility Tests** - Ensure proper ARIA attributes and semantic HTML

### Running Tests

```bash
# Run all tests once
npm test

# Run tests in watch mode (reruns on file changes)
npm run test:watch

# Generate coverage report
npm run test:coverage
```

### Test Files

- `src/__tests__/page.test.tsx` - Tests for the main page component
- `src/__tests__/layout.test.tsx` - Tests for the root layout
- `src/__tests__/integration.test.tsx` - Integration and accessibility tests

The tests cover:
- Component rendering and content
- HeroUI component integration
- Responsive layout structure
- Accessibility attributes
- User interactions

## Customization

### Theme Configuration

HeroUI themes can be customized in `tailwind.config.js`:

```javascript
import {heroui} from "@heroui/react";

export default {
  plugins: [
    heroui({
      themes: {
        light: {
          colors: {
            primary: "#your-color",
            // ... more customizations
          }
        }
      }
    })
  ]
}
```

### Dark Mode

Dark mode is supported out of the box. Toggle it by adding/removing the `dark` class on the `html` element.

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.